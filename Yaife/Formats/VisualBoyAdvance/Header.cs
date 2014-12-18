using System;
using System.ComponentModel;
using System.IO;
using Yaife.Utilities;

namespace Yaife.Formats.VisualBoyAdvance
{
	public class Header
	{
		[Description("Unique file type signature, should be 56 42 4D 1A."),
		Category("Movie File"),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] Signature { get; set; }

		[Description("The major version number of the movie file type, should be 1."),
		Category("Movie File")]
		public uint MajorVersion { get; set; }

		[Description("A unique identifier, also used as the recording time in Unix epoch format."),
		Category("Movie File")]
		public int UID { get; set; }

		[Description("Number of frames in the movie file."),
		Category("Movie File")]
		public uint FrameCount { get; set; }

		[Description("Amount of rerecords (loadstates) made during the recording of the movie."),
		Category("Movie File")]
		public uint RerecordCount { get; set; }

		[Description("How the movie should start."),
		Category("Movie File")]
		public StartFrom StartFrom { get; set; }

		[Description("The controllers connected to the system. Super Gameboy allows up to four controllers (with multitap)."),
		Category("Movie File")]
		public ConnectedControllers ConnectedControllers { get; set; }

		[Description("The system the game is for."),
		Category("Game")]
		public System System { get; set; }

		[Description("Whether an external BIOS file was used for the movie (GBA only)."),
		Category("Configuration")]
		public bool UseBIOS { get; set; }

		[Description("Whether the BIOS intro was skipped (True) or included in the movie file (False). Only applies to GBA and use of an external BIOS file."),
		Category("Configuration")]
		public bool SkipBIOS { get; set; }

		[Description("Whether the real-time clock feature was used for this movie."),
		Category("Configuration")]
		public bool EnableRTC { get; set; }

		[Description("If True, the movie was made with Null Input Kludge on (does not apply to GBA)."),
		Category("Configuration")]
		public bool InputHack { get; set; }

		[Description("Whether the movie uses the new GBA timing (GBA only)."),
		Category("Configuration")]
		public bool ReduceLag { get; set; }

		[Description("Whether the movie uses the new GBC HDMA5 timing (GBC only)."),
		Category("Configuration")]
		public bool HDMA5TimingFix { get; set; }

		[Description("Whether the movie was recorded with Echo RAM Fix (does not apply to GBA)."),
		Category("Configuration")]
		public bool EchoRAMFix { get; set; }

		[Description("Whether the movie was recorded with SRAM Init Fix (GBA only)."),
		Category("Configuration")]
		public bool SRAMInitFix { get; set; }

		[Description("The save type used by the emulator for this movie."),
		Category("Configuration")]
		public SaveType WinSaveType { get; set; }

		[Description("The size of the flash memory used by the emulator for this movie."),
		Category("Configuration")]
		public FlashSize WinFlashSize { get; set; }

		[Description("The emulator type used for this movie."),
		Category("Configuration")]
		public EmulatorType GBEmulatorType { get; set; }

		[Description("Internal game name of the ROM (12 characters)."),
		Category("Game")]
		public string GameName { get; set; }

		[Description("The minor version number of the movie file type, latest is 1."),
		Category("Movie File")]
		public byte MinorVersion { get; set; }

		[Description("Internal CRC of the ROM."),
		Category("Game")]
		public byte InternalROMChecksum { get; set; }

		[Description("A 16-bit CRC of the BIOS if GBA, otherwise the internal CRC of the ROM."),
		Category("Game")]
		public ushort ROMChecksum { get; set; }

		[Description("The Game Code of the ROM if GBA, or the Unit Code otherwise."),
		Category("Game")]
		public uint GameCode { get; set; }

		// Not editable
		public uint SRAMOffset;
		public uint ControllerOffset;

		// Implicit
		[Description("The savestate or SRAM for the movie to start from. Not relevant if the movie starts from power on."),
		Category("Movie File")]
		public byte[] StartSave { get; set; }

		// Rest
		[Description("The name of the author. 64 characters max."),
		Category("Movie File")]
		public string Author { get; set; }

		[Description("A description of the movie, provided by the author. 128 characters max."),
		Category("Movie File")]
		public string Description { get; set; }

		public void Read(FileStream stream)
		{
			var reader = new BinaryReader(stream);

			Signature            = reader.ReadBytes(4);
			MajorVersion         = reader.ReadUInt32();
			UID                  = reader.ReadInt32();
			FrameCount           = reader.ReadUInt32();
			RerecordCount        = reader.ReadUInt32();
			StartFrom            = (StartFrom)reader.ReadByte();
			ConnectedControllers = (ConnectedControllers)reader.ReadByte();
			System               = (System)reader.ReadByte();
			
			var flags      = reader.ReadByte();
			UseBIOS        = ((flags & 0x01) > 0);
			SkipBIOS       = ((flags & 0x02) > 0);
			EnableRTC      = ((flags & 0x04) > 0);
			InputHack      = ((flags & 0x08) > 0);
			ReduceLag      = ((flags & 0x10) > 0);
			HDMA5TimingFix = ((flags & 0x20) > 0);
			EchoRAMFix     = ((flags & 0x40) > 0);
			SRAMInitFix    = ((flags & 0x80) > 0);

			WinSaveType     = (SaveType)reader.ReadUInt32();
			WinFlashSize    = (FlashSize)reader.ReadUInt32();
			GBEmulatorType  = (EmulatorType)reader.ReadUInt32();
			GameName        = reader.ReadASCII(12);
			MinorVersion    = reader.ReadByte();
			InternalROMChecksum = reader.ReadByte();
			ROMChecksum     = reader.ReadUInt16();
			GameCode        = reader.ReadUInt32();

			SRAMOffset       = reader.ReadUInt32();
			ControllerOffset = reader.ReadUInt32();

			Author      = reader.ReadASCII(64);
			Description = reader.ReadASCII(128);

			if (StartFrom != StartFrom.PowerOn)
				StartSave = reader.ReadBytes((int)(SRAMOffset - ControllerOffset));
		}

		public void Write(FileStream stream)
		{
			var writer = new BinaryWriter(stream);

			writer.Write(Signature);
			writer.Write(MajorVersion);
			writer.Write(UID);
			writer.Write(FrameCount);
			writer.Write(RerecordCount);
			writer.Write((byte)StartFrom);
			writer.Write((byte)ConnectedControllers);
			writer.Write((byte)System);

			byte flags = 0;
			flags |= (byte)(UseBIOS        ? 0x01 : 0x00);
			flags |= (byte)(SkipBIOS       ? 0x02 : 0x00);
			flags |= (byte)(EnableRTC      ? 0x04 : 0x00);
			flags |= (byte)(InputHack      ? 0x08 : 0x00);
			flags |= (byte)(ReduceLag      ? 0x10 : 0x00);
			flags |= (byte)(HDMA5TimingFix ? 0x20 : 0x00);
			flags |= (byte)(EchoRAMFix     ? 0x40 : 0x00);
			flags |= (byte)(SRAMInitFix    ? 0x80 : 0x00);
			writer.Write(flags);

			writer.Write((uint)WinSaveType);
			writer.Write((uint)WinFlashSize);
			writer.Write((uint)GBEmulatorType);
			writer.WriteASCII(GameName, 12);
			writer.Write(MinorVersion);
			writer.Write(InternalROMChecksum);
			writer.Write(ROMChecksum);
			writer.Write(GameCode);

			if (StartFrom == StartFrom.PowerOn)
			{
				SRAMOffset       = 0;
				ControllerOffset = 0x100;
			}
			else
			{
				SRAMOffset       = 0x100;
				ControllerOffset = (uint)(SRAMOffset + StartSave.Length);
			}

			writer.Write(SRAMOffset);
			writer.Write(ControllerOffset);

			writer.WriteASCII(Author, 64);
			writer.WriteASCII(Description, 128);

			if (StartFrom != StartFrom.PowerOn)
				writer.Write(StartSave);
		}
	}

	public enum StartFrom : byte
	{
		PowerOn   = 0x00,
		Savestate = 0x01,
		SRAM      = 0x02
	}

	[Flags]
	public enum ConnectedControllers : byte
	{
		Port1 = 0x01,
		Port2 = 0x02,
		Port3 = 0x04,
		Port4 = 0x08
	}

	public enum System : byte
	{
		Gameboy        = 0x00,
		GameboyAdvance = 0x01,
		GameboyColor   = 0x02,
		SuperGameboy   = 0x03
	}

	public enum SaveType : uint
	{
		Automatic    = 0,
		EEPROM       = 1,
		SRAM         = 2,
		Flash        = 3,
		EEPROMSensor = 4,
		None         = 5
	}

	public enum FlashSize : uint
	{
		Flash512k = 0x10000,
		Flash1M   = 0x20000
	}

	public enum EmulatorType : uint
	{
		Automatic               = 0,
		GameboyColor            = 1,
		SuperGameboy            = 2,
		Gameboy                 = 3,
		GameboyAdvance          = 4,
		GameboyColorWithBorders = 5
	}
}
