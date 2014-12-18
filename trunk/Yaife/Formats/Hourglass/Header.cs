using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Yaife.Utilities;

namespace Yaife.Formats.Hourglass
{
	public class Header
	{
		[Description("Unique file type signature, should be 66 54 77 02."),
		Category("Movie File"),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] Signature { get; set; }

		[Description("Amount of frames in the movie file."),
		Category("Movie File")]
		public uint InputFrames { get; set; }

		[Description("Amount of rerecords in the movie."),
		Category("Movie File")]
		public uint RerecordCount { get; set; }

		[Description("ASCII encoded keyboard layout."),
		Category("System")]
		public string KeyboardLayout { get; set; }

		[Description("Frames per second at which to play back the movie."),
		Category("Movie File")]
		public uint FPS { get; set; }

		[Description("System time at movie start."),
		Category("System")]
		public uint SystemTime { get; set; }

		[Description("CRC32 of the game's executable."),
		Category("Game")]
		public uint CRC32 { get; set; }

		[Description("Size (in bytes) of the game's executable."),
		Category("Game")]
		public uint ExeSize { get; set; }

		[Description("Filename (including extension) of the game's executable."),
		Category("Game")]
		public string ExeName { get; set; }

		[Description("Set of 16 system times checked at regular intervals to detect movie desyncs. Set to 0 to ignore."),
		Category("System")]
		public uint[] DesyncDetection { get; set; }

		[Description("Hourglass revision number used to create the movie file."),
		Category("Movie File")]
		public uint HourglassRevision { get; set; }

		[Description("Command line parameters passed to the game."),
		Category("Game")]
		public string CommandLine { get; set; }

		public Header(FileStream stream)
		{
			var reader = new BinaryReader(stream);

			Signature = reader.ReadBytes(4);

			InputFrames = reader.ReadUInt32();
			RerecordCount = reader.ReadUInt32();

			KeyboardLayout = reader.ReadASCII(8);

			FPS = reader.ReadUInt32();
			SystemTime = reader.ReadUInt32();
			CRC32 = reader.ReadUInt32();
			ExeSize = reader.ReadUInt32();

			ExeName = reader.ReadASCII(48);

			DesyncDetection = new uint[16];
			for (int i = 0; i < 16; i++)
				DesyncDetection[i] = reader.ReadUInt32();

			HourglassRevision = reader.ReadUInt32();

			CommandLine = reader.ReadASCII(160);
		}

		public void Write(FileStream stream)
		{
			var writer = new BinaryWriter(stream);

			writer.Write(Signature, 0, 4);
			writer.Write(InputFrames);
			writer.Write(RerecordCount);
			writer.WriteASCII(KeyboardLayout.PadLeft(8, '0'), 8);
			writer.Write(FPS);
			writer.Write(SystemTime);
			writer.Write(CRC32);
			writer.Write(ExeSize);
			writer.WriteASCII(ExeName, 48);

			for (int i = 0; i < 16; i++)
				writer.Write(DesyncDetection[i]);

			writer.Write(HourglassRevision);
			writer.WriteASCII(CommandLine, 160);
		}
	}
}
