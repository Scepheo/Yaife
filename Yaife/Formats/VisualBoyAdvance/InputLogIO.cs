using System.Collections.Generic;
using System.IO;

namespace Yaife.Formats.VisualBoyAdvance
{
	public static class InputLogIO
	{
		public static InputLog Read(FileStream stream, ConnectedControllers controllers)
		{
			int dataSize = 0;
			var headers = new List<string>();
			var int8 = (byte)controllers;

			for (int i = 1; i < 0x100; i <<= 1)
			{
				var thisBit = (byte)(int8 & i);

				if (thisBit > 0)
				{
					dataSize += 2;
					headers.Add(((ConnectedControllers)thisBit).ToString());
				}
			}

			var list = new List<Frame>();
			var endOfStream = false;
			var data = new byte[dataSize];

			while (!endOfStream)
			{
				endOfStream = stream.Read(data, 0, dataSize) < dataSize;

				if (!endOfStream)
					list.Add(new Frame(data));
			}

			return new InputLog(headers.ToArray(), list);
		}

		public static void Write(FileStream stream, InputLog log)
		{
			foreach (var str in log.Log)
			{
				var frame = new Frame();
				frame.Parse(str);
				var buffer = frame.GetBytes();
				stream.Write(buffer, 0, buffer.Length);
			}
		}
	}
}
