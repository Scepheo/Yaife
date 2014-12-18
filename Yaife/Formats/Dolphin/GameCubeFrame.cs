using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaife.Formats.Dolphin
{
	[Flags]
	public enum GCButton : short
	{
		None  = 0x0000,
		Left  = 0x0001,
		Right = 0x0002,
		Down  = 0x0004,
		Up    = 0x0008,
		Z     = 0x0010,
		R     = 0x0020,
		L     = 0x0040,
		A     = 0x0100,
		B     = 0x0200,
		X     = 0x0400,
		Y     = 0x0800,
		Start = 0x1000,
		Disc  = 0x2000
	}

	public class GameCubeFrame : IFrame
	{
		GCButton Button;
		byte TriggerL, TriggerR;
		sbyte AnalogX, AnalogY;
		sbyte CStickX, CStickY;

		public GameCubeFrame() : this(new byte[8]) {}
		
		public GameCubeFrame(byte[] data)
		{
			this.Button = (GCButton)(data[0] | (data[1] << 8));

			this.TriggerL = data[2];
			this.TriggerR = data[3];
			this.AnalogX  = (sbyte)(data[4] - 128);
			this.AnalogY  = (sbyte)(data[5] - 128);
			this.CStickX  = (sbyte)(data[6] - 128);
			this.CStickY  = (sbyte)(data[7] - 128);
		}

		public string[] ToStrings()
		{
			var result = new string[5];

			result[0] = Button.ToString();
			result[1] = String.Format("{0:d3}", TriggerL);
			result[2] = String.Format("{0:d3}", TriggerR);
			result[3] = String.Format("{0,4:d3},{1,4:d3}", AnalogX, AnalogY);
			result[4] = String.Format("{0,4:d3},{1,4:d3}", CStickX, CStickY);

			return result;
		}

		public void Parse(string[] str)
		{
			// Parse buttons first, accepting empty strings as None
			if (String.IsNullOrWhiteSpace(str[0]))
				Button = GCButton.None;
			else
				Button = (GCButton)Enum.Parse(typeof(GCButton), str[0]);

			// Parse triggers
			TriggerL = byte.Parse(str[1]);
			TriggerR = byte.Parse(str[2]);

			// Parse analogs
			var analogSplit = str[3].Split(',');
			AnalogX = sbyte.Parse(analogSplit[0]);
			AnalogY = sbyte.Parse(analogSplit[1]);

			// Parse C-Stick
			var cStickSplit = str[4].Split(',');
			CStickX = sbyte.Parse(cStickSplit[0]);
			CStickY = sbyte.Parse(cStickSplit[1]);
		}

		public byte[] GetBytes()
		{
			var data = new byte[8];
			var int16 = (short)Button;

			data[0] = (byte)(int16 & 0xFF);
			data[1] = (byte)(int16 >> 8);
			data[2] = TriggerL;
			data[3] = TriggerR;
			data[4] = (byte)(AnalogX + 128);
			data[5] = (byte)(AnalogY + 128);
			data[6] = (byte)(CStickX + 128);
			data[7] = (byte)(CStickY + 128);

			return data;
		}

		public bool IsEmpty()
		{
			return
			(
				Button == GCButton.None &&
				TriggerL != 0 && TriggerR != 0 &&
				AnalogX != 0 && AnalogY != 0 &&
				CStickX != 0 && CStickY != 0
			);
		}
	}
}
