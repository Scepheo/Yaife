using System;
using System.Text;

namespace Yaife.Formats.PSXjin
{
	public abstract class PSXController
	{
		public abstract void Parse(string[] str);
		public abstract string[] ToStrings();
		public abstract byte[] GetBytes(bool text);
		public abstract bool IsEmpty();

		public static PSXController GetController(PSXControllerType type, byte[] data, bool text)
		{
			switch (type)
			{
				case PSXControllerType.StandardController:
					return new StandardController(data, text);
				case PSXControllerType.Mouse:
					return new Mouse(data, text);
				case PSXControllerType.Joystick:
				case PSXControllerType.AnalogController:
					return new AnalogController(data, text);
				default:
					return new StandardController(new byte[2], false);
			}
		}

		public static string[] GetHeaders(PSXControllerType type)
		{
			switch (type)
			{
				case PSXControllerType.Mouse:
					return new string[] { "Buttons", "Movement" };
				case PSXControllerType.Joystick:
				case PSXControllerType.AnalogController:
					return new string[] { "Buttons", "Left", "Right" };
				default:
					return new string[] { "Buttons" };
			}
		}
	}

	public enum PSXControllerType : byte
	{
		Mouse = 1,
		Negcon = 2,
		KonamiGun = 3,
		StandardController = 4,
		Joystick = 5,
		NamcoGun = 6,
		AnalogController = 7
	}

	[Flags]
	public enum Buttons : ushort
	{
		None     = 0x0000,
		Select   = 0x0001,
		Unknown1 = 0x0002,
		Unknown2 = 0x0004,
		Start    = 0x0008,
		Up       = 0x0010,
		Right    = 0x0020,
		Down     = 0x0040,
		Left     = 0x0080,
		L2       = 0x0100,
		R2       = 0x0200,
		L1       = 0x0400,
		R1       = 0x0800,
		Triangle = 0x1000,
		Circle   = 0x2000,
		Cross    = 0x4000,
		Square   = 0x8000
	}

	public class StandardController : PSXController
	{
		private Buttons buttons;

		public StandardController(byte[] data, bool text)
		{
			if (text)
			{
				var str = ASCIIEncoding.ASCII.GetString(data);
				ushort flags = 0;

				for (int i = 0; i < 13; i++)
				{
					flags <<= 1;

					if (str[i] != '.')
						flags |= 1;
				}

				this.buttons = (Buttons)flags;
			}
			else
			{
				var int16 = (ushort)(data[0] | (data[1] << 8));
				this.buttons = (Buttons)int16;
			}
		}

		public override void Parse(string[] str)
		{
			this.buttons = (Buttons)Enum.Parse(typeof(Buttons), str[0]);
		}

		public override string[] ToStrings()
		{
			return new string[] { buttons.ToString() };
		}

		public override byte[] GetBytes(bool text)
		{
			var result = new byte[2];

			var int16 = (ushort)buttons;
			result[0] = (byte)(int16 & 0xFF);
			result[1] = (byte)(int16 >> 8);

			return result;
		}

		public override bool IsEmpty()
		{
			return (buttons == Buttons.None);
		}
	}

	public class Mouse : PSXController
	{
		private Buttons buttons;
		private sbyte dX, dY;

		public Mouse(byte[] data, bool text)
		{
			var x = (ushort)(data[0] | (data[1] << 8));
			this.buttons = (Buttons)x;
			this.dX = (sbyte)(data[2] - 128);
			this.dY = (sbyte)(data[3] - 128);
		}

		public override void Parse(string[] str)
		{
			this.buttons = (Buttons)Enum.Parse(typeof(Buttons), str[0]);
			
			var movementSplit = str[1].Split(',');
			dX = sbyte.Parse(movementSplit[0]);
			dY = sbyte.Parse(movementSplit[1]);
		}

		public override string[] ToStrings()
		{
			var result = new string[2];

			result[0] = buttons.ToString();
			result[1] = String.Format("{0,4:d3},{1,4:d3}", dX, dY);

			return result;
		}

		public override byte[] GetBytes(bool text)
		{
			var result = new byte[4];

			var int16 = (ushort)buttons;
			result[0] = (byte)(int16 & 0xFF);
			result[1] = (byte)(int16 >> 8);
			result[2] = (byte)(dX + 128);
			result[3] = (byte)(dY + 128);

			return result;
		}

		public override bool IsEmpty()
		{
			return (buttons == Buttons.None
				&& dX == 0 && dY == 0);
		}
	}

	public class AnalogController : PSXController
	{
		private Buttons buttons;
		private sbyte leftX, leftY, rightX, rightY;

		public AnalogController(byte[] data, bool text)
		{
			var x = (ushort)(data[0] | (data[1] << 8));
			this.buttons = (Buttons)x;
			this.leftX = (sbyte)(data[2] - 128);
			this.leftY = (sbyte)(data[3] - 128);
			this.rightX = (sbyte)(data[4] - 128);
			this.rightY = (sbyte)(data[5] - 128);
		}

		public override void Parse(string[] str)
		{
			this.buttons = (Buttons)Enum.Parse(typeof(Buttons), str[0]);

			var leftSplit = str[1].Split(',');
			leftX = sbyte.Parse(leftSplit[0]);
			leftY = sbyte.Parse(leftSplit[1]);

			var rightSplit = str[2].Split(',');
			rightX = sbyte.Parse(rightSplit[0]);
			rightY = sbyte.Parse(rightSplit[1]);
		}

		public override string[] ToStrings()
		{
			var result = new string[3];

			result[0] = buttons.ToString();
			result[1] = String.Format("{0,4:d3},{1,4:d3}", leftX, leftY);
			result[2] = String.Format("{0,4:d3},{1,4:d3}", rightX, rightY);

			return result;
		}

		public override byte[] GetBytes(bool text)
		{
			var result = new byte[6];

			var int16 = (ushort)buttons;
			result[0] = (byte)(int16 & 0xFF);
			result[1] = (byte)(int16 >> 8);
			result[2] = (byte)(leftX + 128);
			result[3] = (byte)(leftY + 128);
			result[4] = (byte)(rightX + 128);
			result[5] = (byte)(rightY + 128);

			return result;
		}

		public override bool IsEmpty()
		{
			return (buttons == Buttons.None
				&& leftX == 0 && leftY == 0
				&& rightX == 0 && rightY == 0);
		}
	}
}
