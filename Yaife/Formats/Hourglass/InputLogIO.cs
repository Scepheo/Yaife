using System.Collections.Generic;
using System.IO;

namespace Yaife.Formats.Hourglass
{
	public static class InputLogIO
	{
		public static InputLog Read(FileStream stream)
		{
			var buffer = new byte[8];
			var log = new List<Frame>();

			while (stream.Read(buffer, 0, 8) > 0)
				log.Add(new Frame(buffer));

			return new InputLog(new string[] { "Input" }, log);
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
