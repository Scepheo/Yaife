using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaife.Formats.Dolphin
{
	public class WiiMoteFrame : IFrame
	{
		PacketType PacketType = PacketType.Buttons;
		WiiMoteButtons Buttons = WiiMoteButtons.None;
		byte[] Data = new byte[0];
		
		public WiiMoteFrame() {}

		public WiiMoteFrame(BinaryReader stream)
		{
			// We can skip length, as this is implicit
			stream.ReadByte();

			if (stream.ReadByte() != 0xA1)
				throw new Exception("Input stream signature does not match.");

			this.PacketType = (PacketType)stream.ReadByte();

			if (PacketType != PacketType.Extension_21)
				this.Buttons = (WiiMoteButtons)stream.ReadUInt16();

			Data = stream.ReadBytes(GetDataSize(PacketType));
		}

		public string[] ToStrings()
		{
			var result = new string[3];

			result[0] = PacketType.ToString();

			if (PacketType == PacketType.Extension_21)
				result[1] = "";
			else
				result[1] = Buttons.ToString();

			// TODO: Make this prettier
			switch (PacketType)
			{
				default:
					result[2] = "";
					foreach (var b in Data)
						result[2] += b.ToString("X2") + " ";
					break;
				case PacketType.StatusInformation:
					result[2] = String.Join(" ",
						"batteryEmpty:" + trueFalse((Data[0] & 0x01) > 0),
						"extension:"    + onOff((Data[0] & 0x02) > 0),
						"speaker:"      + onOff((Data[0] & 0x04) > 0),
						"irCamera:"     + onOff((Data[0] & 0x08) > 0),
						"led1"          + onOff((Data[0] & 0x10) > 0),
						"led2"          + onOff((Data[0] & 0x20) > 0),
						"led3"          + onOff((Data[0] & 0x40) > 0),
						"led4"          + onOff((Data[0] & 0x80) > 0),
						"batteryLevel:" + Data[3]);
					break;
				case PacketType.ReadMemory:
					result[2] = String.Join(" ",
						"error:" + ((WiiMoteReadError)(Data[0] >> 4)).ToString(),
						"size:" + (Data[0] & 0x0F).ToString(),
						"address:" + ((Data[1] << 8) | Data[2]).ToString("X4"),
						"data:" + hexString(Data, 3));
					break;
				case PacketType.Acknowledge:
					result[2] = String.Join(" ",
						"report:" + Data[0].ToString(),
						"error:" + ((WiiMoteError)Data[1]).ToString());
					break;
				case PacketType.Buttons:
					result[2] = "";
					break;
			}

			return result;
		}

		private string hexString(byte[] data, int offset = 0)
		{
			return hexString(data, offset, data.Length - offset);
		}

		private string hexString(byte[] data, int offset, int length)
		{
			var sb = new StringBuilder(2 * length);

			for (int i = 0; i < length; i++)
				sb.Append(data[offset + i].ToString("X2"));

			return sb.ToString();
		}

		private string onOff(bool value)
		{
			return value ? "on" : "off";
		}

		private string trueFalse(bool value)
		{
			return value ? "true" : "false";
		}

		public void Parse(string[] str)
		{
			// Parse packet type
			PacketType = (PacketType)Enum.Parse(typeof(PacketType), str[0]);

			// Parse buttons, accepting empty strings as None
			if (PacketType == PacketType.Extension_21 || String.IsNullOrWhiteSpace(str[1]))
				Buttons = WiiMoteButtons.None;
			else
				Buttons = (WiiMoteButtons)Enum.Parse(typeof(WiiMoteButtons), str[1]);

			// Parse rest of data
			var split = str[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			Data = new byte[split.Length];

			for (int i = 0; i < Data.Length; i++)
				Data[i] = byte.Parse(split[i], NumberStyles.HexNumber);
		}

		public byte[] GetBytes()
		{
			var list = new List<byte>();

			list.Add(0xA1);
			list.Add((byte)PacketType);

			if (PacketType != PacketType.Extension_21)
			{
				var uint16 = (ushort)Buttons;
				list.Add((byte)(uint16 & 0xFF));
				list.Add((byte)(uint16 >> 8));
			}

			list.AddRange(Data);
			list.Insert(0, (byte)list.Count);

			return list.ToArray();
		}

		public bool IsEmpty()
		{
			// I'm pretty sure the concept of an empty wiimote packet isn't useful.
			return false;
		}

		public int GetDataSize(PacketType type)
		{
			switch (type)
			{
				default:
				case PacketType.Buttons:
					return 0;
				case PacketType.Acknowledge:
					return 2;
				case PacketType.Accelerometer:
					return 3;
				case PacketType.StatusInformation:
					return 4;
				case PacketType.Extension_8:
					return 8;
				case PacketType.Accelerometer_IR:
					return 15;
				case PacketType.ReadMemory:
				case PacketType.Extension_19:
				case PacketType.Accelerometer_Extension_16:
				case PacketType.IR_Extension_9:
				case PacketType.Accelerometer_IR_Extension_6:
				case PacketType.Accelerometer_IR_a:
				case PacketType.Accelerometer_IR_b:
					return 19;
				case PacketType.Extension_21:
					return 21;
			}
		}
	}

	public enum PacketType : byte
	{
		StatusInformation            = 0x20,
		ReadMemory                   = 0x21,
		Acknowledge                  = 0x22,
		Buttons                      = 0x30,
		Accelerometer                = 0x31,
		Extension_8                  = 0x32,
		Accelerometer_IR             = 0x33,
		Extension_19                 = 0x34,
		Accelerometer_Extension_16   = 0x35,
		IR_Extension_9               = 0x36,
		Accelerometer_IR_Extension_6 = 0x37,
		Extension_21                 = 0x3D,
		Accelerometer_IR_a           = 0x3E,
		Accelerometer_IR_b           = 0x3F
	}

	[Flags]
	public enum WiiMoteButtons : ushort
	{
		None  = 0x0000,
		Left  = 0x0001,
		Right = 0x0002,
		Down  = 0x0004,
		Up    = 0x0008,
		Plus  = 0x0010,
		Two   = 0x0100,
		One   = 0x0200,
		B     = 0x0400,
		A     = 0x0800,
		Minus = 0x1000,
		Home  = 0x8000
	}

	public enum WiiMoteError : byte
	{
		Success  = 0x00,
		Error    = 0x03,
		Unknown1 = 0x04,
		Unknown2 = 0x05,
		Unknown3 = 0x08
	}

	public enum WiiMoteReadError : byte
	{
		Success     = 0x00,
		WriteOnly   = 0x07,
		NonExistent = 0x08
	}
}