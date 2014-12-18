using System.Collections.Generic;
using System.IO;

namespace Yaife.Formats.Dolphin
{
	public static class InputLogIO
	{
		public static InputLog Read(FileStream stream, bool isWii)
		{
			string[] headers;
			List<IFrame> log = new List<IFrame>();

			if (isWii)
			{
				headers = new string[]
				{
					"Packet Type",
					"Buttons",
					"Data"
				};

				var reader = new BinaryReader(stream);

				while (stream.Position < stream.Length)
					log.Add(new WiiMoteFrame(reader));
			}
			else
			{
				headers = new string[]
				{
					"Keys",
					"L-Trigger",
					"R-Trigger",
					"Analog",
					"C-Stick"
				};

				var buffer = new byte[8];

				while (stream.Read(buffer, 0, 8) > 0)
					log.Add(new GameCubeFrame(buffer));
			}

			return new InputLog(headers, log);
		}

		public static void Write(FileStream stream, InputLog log, bool isWii)
		{
			if (isWii)
			{
				foreach (var str in log.Log)
				{
					var frame = new WiiMoteFrame();
					frame.Parse(str);
					var buffer = frame.GetBytes();
					stream.Write(buffer, 0, buffer.Length);
				}
			}
			else
			{
				foreach (var str in log.Log)
				{
					var frame = new GameCubeFrame();
					frame.Parse(str);
					var buffer = frame.GetBytes();
					stream.Write(buffer, 0, buffer.Length);
				}
			}
		}
	}
}
