using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Yaife.Utilities;

namespace Yaife.Formats.Dolphin
{
	public class Header
	{
		[Description("Unique file type identifier (should be 44 54 4D 1A)."),
		Category("Movie File"),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] FileType { get; set; }

		[Description("Game identifier."),
		Category("Game")]
		public string GameID { get; set; }

		[Description("Indicates whether the movie is for a Wii game."),
		Category("Game")]
		public bool IsWii { get; set; }

		[Description("Connected controllers, use commas to separate multiple values.")]
		public Controller Controllers { get; set; }

		[Description("Indicated whether the movie starts from a savestate (True) or from powerup (False)."),
		Category("Movie File")]
		public bool StartsFromSaveState { get; set; }

		[Description("Number of video frames in the movie."),
		Category("Movie File")]
		public ulong FrameCount { get; set; }

		[Description("Number of input frames in the movie."),
		Category("Movie File")]
		public ulong InputCount { get; set; }

		[Description("Number of lag frames in the movie."),
		Category("Movie File")]
		public ulong LagCount { get; set; }

		[Description("[Not used] Unique movie file identifier: MD5 of SystemStartTime + GameID."),
		Category("Movie File")]
		public ulong UniqueID { get; set; }

		[Description("Number of rerecords in the movie."),
		Category("Movie File")]
		public uint RerecordCount { get; set; }

		[Description("Name of the movie's author."),
		Category("Movie File")]
		public string Author { get; set; }

		[Description("Name of the video backend.")]
		public string VideoBackend { get; set; }

		[Description("Name of the audio emulator.")]
		public string AudioEmulator { get; set; }

		[Description("MD5 Hash of the game's ISO."),
		Category("Game"),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] MD5 { get; set; }

		[Description("Unix time (seconds since 1970) of movie start, used for real-time clock)."),
		Category("Movie File")]
		public ulong SystemStartTime { get; set; }

		[Description("Indicates whether to load/save the included configuration settings."),
		Category("Configuration")]
		public bool SaveConfig { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool SkipIdle { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool DualCore { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool Progressive { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool DSPHLE { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool FastDiscSpeed { get; set; }

		[Description("The core used to emulate the CPU."),
		Category("Configuration")]
		public CPUCore CPUCore { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool EFBAccessEnable { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool EFBCopyEnable { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool CopyEFBToTexture { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool EFBCopyCacheEnable { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool EFBEmulateFormatChanges { get; set; }

		[Description("Whether an external frame buffer is used."),
		Category("Configuration")]
		public bool UseXFB { get; set; }

		[Description("If an external frame buffer is used, whether to use a real one (True) or a virtual one (False)."),
		Category("Configuration")]
		public bool UseRealXFB { get; set; }

		[Description("Plugged in memory cards, use commas to separate multiple values."),
		Category("Configuration")]
		public MemoryCard MemoryCards { get; set; }

		[Description("Whether to clear the memory card before movie playback."),
		Category("Configuration")]
		public bool ClearSave { get; set; }

		[Description("Connected bongo controllers, use commas to separate multiple values."),
		Category("Configuration")]
		public Controller Bongos { get; set; }

		[Description(""),
		Category("Configuration")]
		public bool SyncGPU { get; set; }

		[Description("Whether the movie was recorded using Net Play."),
		Category("Configuration")]
		public bool NetPlay { get; set; }

		[Description("Reserved for additional configuration options."),
		Category("Configuration"),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] ReservedConfiguration { get; set; }

		[Description("Name of ISO file to switch to, for two disc games."),
		Category("Game")]
		public string DiscChangeName { get; set; }

		[Description("Revision hash, as used by Git."),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] GitRevisionHash { get; set; }

		[Description("")]
		public uint DSPiromHash { get; set; }

		[Description("")]
		public uint DSPcoefHash { get; set; }

		[Description("Number of CPU ticks elapsed in the movie."),
		Category("Movie File")]
		public ulong TickCount { get; set; }

		[Description("Reserved bytes, padding to 256 bytes."),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] ReservedPadding { get; set; }

		public void Read(FileStream stream)
		{
			var reader = new BinaryReader(stream);

			FileType                = reader.ReadBytes(4);
			GameID                  = reader.ReadUTF8(6);
			IsWii                   = reader.ReadBoolean();
			Controllers             = (Controller)reader.ReadByte();
			StartsFromSaveState     = reader.ReadBoolean();
			FrameCount              = reader.ReadUInt64();
			InputCount              = reader.ReadUInt64();
			LagCount                = reader.ReadUInt64();
			UniqueID                = reader.ReadUInt64();
			RerecordCount           = reader.ReadUInt32();
			Author                  = reader.ReadUTF8(32);
			VideoBackend            = reader.ReadUTF8(16);
			AudioEmulator           = reader.ReadUTF8(16);
			MD5                     = reader.ReadBytes(16);
			SystemStartTime         = reader.ReadUInt64();
			SaveConfig              = reader.ReadBoolean();
			SkipIdle                = reader.ReadBoolean();
			DualCore                = reader.ReadBoolean();
			Progressive             = reader.ReadBoolean();
			DSPHLE                  = reader.ReadBoolean();
			FastDiscSpeed           = reader.ReadBoolean();
			CPUCore                 = (CPUCore)reader.ReadByte();
			EFBAccessEnable         = reader.ReadBoolean();
			EFBCopyEnable           = reader.ReadBoolean();
			CopyEFBToTexture        = reader.ReadBoolean();
			EFBCopyCacheEnable      = reader.ReadBoolean();
			EFBEmulateFormatChanges = reader.ReadBoolean();
			UseXFB                  = reader.ReadBoolean();
			UseRealXFB              = reader.ReadBoolean();
			MemoryCards             = (MemoryCard)reader.ReadByte();
			ClearSave               = reader.ReadBoolean();
			Bongos                  = (Controller)reader.ReadByte();
			SyncGPU                 = reader.ReadBoolean();
			NetPlay                 = reader.ReadBoolean();
			ReservedConfiguration   = reader.ReadBytes(13);
			DiscChangeName          = reader.ReadUTF8(40);
			GitRevisionHash         = reader.ReadBytes(20);
			DSPiromHash             = reader.ReadUInt32();
			DSPcoefHash             = reader.ReadUInt32();
			TickCount               = reader.ReadUInt64();
			ReservedPadding         = reader.ReadBytes(11);
		}

		public void Write(FileStream stream)
		{
			var writer = new BinaryWriter(stream);

			writer.Write(FileType);
			writer.WriteUTF8(GameID, 6);
			writer.Write(IsWii);
			writer.Write((byte)Controllers);
			writer.Write(StartsFromSaveState);
			writer.Write(FrameCount);
			writer.Write(InputCount);
			writer.Write(LagCount);
			writer.Write(UniqueID);
			writer.Write(RerecordCount);
			writer.WriteUTF8(Author, 32);
			writer.WriteUTF8(VideoBackend, 16);
			writer.WriteUTF8(AudioEmulator, 16);
			writer.Write(MD5);
			writer.Write(SystemStartTime);
			writer.Write(SaveConfig);
			writer.Write(SkipIdle);
			writer.Write(DualCore);
			writer.Write(Progressive);
			writer.Write(DSPHLE);
			writer.Write(FastDiscSpeed);
			writer.Write((byte)CPUCore);
			writer.Write(EFBAccessEnable);
			writer.Write(EFBCopyEnable);
			writer.Write(CopyEFBToTexture);
			writer.Write(EFBCopyCacheEnable);
			writer.Write(EFBEmulateFormatChanges);
			writer.Write(UseXFB);
			writer.Write(UseRealXFB);
			writer.Write((byte)MemoryCards);
			writer.Write(ClearSave);
			writer.Write((byte)Bongos);
			writer.Write(SyncGPU);
			writer.Write(NetPlay);
			writer.Write(ReservedConfiguration);
			writer.WriteUTF8(DiscChangeName, 40);
			writer.Write(GitRevisionHash);
			writer.Write(DSPiromHash);
			writer.Write(DSPcoefHash);
			writer.Write(TickCount);
			writer.Write(ReservedPadding);
		}
	}

	public enum CPUCore : byte
	{
		Interpreter = 0,
		JIT = 1,
		JITIL = 2
	}

	[Flags]
	public enum MemoryCard : byte
	{
		None  = 0x00,
		SlotA = 0x01,
		SlotB = 0x02
	}

	[Flags]
	public enum Controller : byte
	{
		None          = 0x00,
		GCController1 = 0x01,
		GCController2 = 0x02,
		GCController3 = 0x04,
		GCController4 = 0x08,
		WiiMote1      = 0x10,
		WiiMote2      = 0x20,
		WiiMote3      = 0x40,
		WiiMote4      = 0x80
	}
}
