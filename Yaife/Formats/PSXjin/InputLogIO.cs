﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Yaife.Formats.PSXjin
{
	public static class InputLogIO
	{
		public static int[] BinaryDataSize = new int[]
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

		public static int[] TextDataSize = new int[]
		{
			00, // None
			12, // Mouse
			00, // Negcon
			00, // KonamiGun
			15, // Standard
			33, // Joystick
			00, // NamcoGun
			33  // AnalogController
		};

		public static InputLog Read(FileStream stream, PSXControllerType port1, PSXControllerType port2, bool text)
		{
			int port1DataSize;
			int port2DataSize;

			if (text)
			{
				port1DataSize = TextDataSize[(int)port1];
				port2DataSize = TextDataSize[(int)port2];
			}
			else
			{
				port1DataSize = BinaryDataSize[(int)port1];
				port2DataSize = BinaryDataSize[(int)port2];
			}

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
					list.Add(new Frame(data1, port1, data2, port2, (byte)control, text));
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

		public static void Write(FileStream stream, InputLog log, bool text)
		{
			foreach (var str in log.Log)
			{
				var frame = new Frame();
				frame.Parse(str);
				var buffer = frame.GetBytes(text);
				stream.Write(buffer, 0, buffer.Length);
			}
		}
	}
}
