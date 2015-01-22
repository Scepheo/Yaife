using System;
using System.Text;

namespace Yaife.Formats.PSXjin
{
	[Flags]
	public enum Console : byte
	{
		None = 0x00,
		Reset = 0x01,
		ToggleCD = 0x02,
		SIOHack = 0x04,
		SPUHack = 0x08,
		Cheats = 0x10,
		ResidentEvilHack = 0x20,
		ParasiteEveHack = 0x40
	}

	public class Frame : IFrame
	{
		private Console console;
		private PSXController controller1, controller2;
		private PSXControllerType type1, type2;

		public Frame()
			: this(new byte[2], PSXControllerType.StandardController, new byte[2], PSXControllerType.StandardController, new byte[1], false)
		{ }

		public Frame(byte[] data1, PSXControllerType port1, byte[] data2, PSXControllerType port2, byte[] control, bool text)
		{
			this.type1 = port1;
			this.type2 = port2;
			this.controller1 = PSXController.GetController(port1, data1, text);
			this.controller2 = PSXController.GetController(port2, data2, text);
			this.console = MakeControl(control, text);
		}

		public Console MakeControl(byte[] data, bool text)
		{
			Console result = Console.None;

			if (text)
			{
				if (data[0] == 1)
					result = Console.Reset;
				if (data[0] == 2)
					result = Console.ToggleCD;
				if (data[0] == 3)
					result = Console.SIOHack;
				if (data[0] == 4)
					result = Console.Cheats;
				if (data[0] == 5)
					result = Console.ResidentEvilHack;
				if (data[0] == 6)
					result = Console.ParasiteEveHack;
			}
			else
			{
				result = (Console)data[0];
			}

			return result;
		}

		public string[] ToStrings()
		{
			var p1 = controller1.ToStrings();
			var p2 = controller2.ToStrings();

			var result = new string[p1.Length + p2.Length + 1];
			Array.Copy(p1, 0, result, 0,         p1.Length);
			Array.Copy(p2, 0, result, p1.Length, p2.Length);
			result[result.Length - 1] = console.ToString();

			return result;
		}

		public void Parse(string[] strings)
		{
			var l1 = PSXController.GetHeaders(type1).Length;
			var l2 = PSXController.GetHeaders(type2).Length;

			var s1 = new string[l1];
			Array.Copy(strings, 0, s1, 0, l1);
			var s2 = new string[l2];
			Array.Copy(strings, l1, s2, 0, l2);

			this.controller1.Parse(s1);
			this.controller2.Parse(s2);
			this.console = (Console)Enum.Parse(typeof(Console), strings[strings.Length - 1]);
		}

		public byte[] GetBytes(bool text)
		{
			byte[] result;

			if (text)
			{
				var str = GetText();
				result = ASCIIEncoding.ASCII.GetBytes(str);
			}
			else
			{
				var b1 = controller1.GetBytes();
				var b2 = controller2.GetBytes();
				result = new byte[b1.Length + b2.Length + 1];

				Array.Copy(b1, 0, result, 0, b1.Length);
				Array.Copy(b2, 0, result, b1.Length, b2.Length);
				result[result.Length - 1] = (byte)console;
			}

			return result;
		}

		public string GetText()
		{
			string result = "";
			result += controller1.GetText();
			result += "|";
			result += controller2.GetText();
			result += "|";

			int control = 0;

			if (console.HasFlag(Console.Reset))
				control = 1;
			if (console.HasFlag(Console.ToggleCD))
				control = 2;
			if (console.HasFlag(Console.SIOHack))
				control = 3;
			if (console.HasFlag(Console.Cheats))
				control = 4;
			if (console.HasFlag(Console.ResidentEvilHack))
				control = 5;
			if (console.HasFlag(Console.ParasiteEveHack))
				control = 6;

			result += control.ToString();
			result += "|\r\n";

			return result;
		}

		public bool IsEmpty()
		{
			return (controller1.IsEmpty()
				&& controller2.IsEmpty()
				&& console == Console.None);
		}
	}
}
