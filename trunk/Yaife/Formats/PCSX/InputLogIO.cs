using System;
using System.Collections.Generic;
using System.IO;

namespace Yaife.Formats.PCSX
{
	public static class InputLogIO
	{
		public static int[] DataSize = new int[]
		{
			0, // None
			4, // Mouse
			0, // Negcon
			0, // KonamiGun
			2, // Standard
			6, // Joystick
			0, // NamcoGun
			6  // AnalogController
		};

		public static InputLog Read(FileStream stream, PSXControllerType port1, PSXControllerType port2)
		{
			int port1DataSize = DataSize[(int)port1];
			int port2DataSize = DataSize[(int)port2];

			var data1 = new byte[port1DataSize];
			var data2 = new byte[port2DataSize];
			int control = 0;

			var list = new List<Frame>();
			var endOfStream = false;

			while (!endOfStream)
			{
				endOfStream = stream.Read(data1, 0, port1DataSize) < port1DataSize;

				if (!endOfStream)
					endOfStream = stream.Read(data2, 0, port2DataSize) < port2DataSize;

				if (!endOfStream)
					endOfStream = (control = stream.ReadByte()) == -1;
				
				if (!endOfStream)
					list.Add(new Frame(data1, port1, data2, port2, (byte)control));
			}

			// Construct the headers
			var port1Headers = PSXController.GetHeaders(port1);
			var port2Headers = PSXController.GetHeaders(port2);

			string[] headers = new string[port1Headers.Length + port2Headers.Length + 1];
			Array.Copy(port1Headers, 0, headers, 0,                   port1Headers.Length);
			Array.Copy(port2Headers, 0, headers, port1Headers.Length, port2Headers.Length);
			headers[headers.Length - 1] = "Console";

			return new InputLog(headers, list);
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
