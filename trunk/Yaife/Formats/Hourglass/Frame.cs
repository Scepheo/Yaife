using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Yaife.Formats.Hourglass
{
	public class Frame : IFrame
	{
		private List<Keys> keys;

		public string[] ToStrings()
		{
			return new string[] { String.Join(", ", keys) };
		}

		public void Parse(string[] str)
		{
			keys.Clear();
			var split = str[0].Split(new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var s in split)
			{
				var keyString = s.Trim();
				keys.Add((Keys)Enum.Parse(typeof(Keys), keyString));
			}
		}

		public Frame(byte[] buffer)
		{
			keys = new List<Keys>();

			for (int i = 0; i < buffer.Length; i++)
				if (buffer[i] > 0)
					keys.Add((Keys)buffer[i]);
		}

		public Frame() : this(new byte[0])
		{
		}

		public byte[] GetBytes()
		{
			var buffer = new byte[8];

			for (int i = 0; i < 8 && i < keys.Count; i++)
				buffer[i] = (byte)keys[i];

			return buffer;
		}
	}
}
