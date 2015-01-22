using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Yaife.Utilities;

namespace Yaife.Formats.PSXjin
{
	public class Header
	{
		[Description("Unique file type signature, should be 50 4A 4D 20."),
		Category("Movie File"),
		TypeConverter(typeof(HexByteConverter))]
		public byte[] Signature { get; set; }

		[Description("Version number of the movie file format. 2 is most recent."),
		Category("Movie File")]
		public uint MovieVersion { get; set; }

		[Description("Version number of the emulator used to record the movie."),
		Category("Movie File")]
		public uint EmulatorVersion { get; set; }

		[Description("Indicates whether the movie starts from an embedded savestate (True) or power on (False)."),
		Category("Movie File")]
		public bool StartsFromSavestate { get; set; }

		[Description("Indicates whether the game is PAL."),
		Category("Game")]
		public bool PAL { get; set; }

		[Description("Indicates whether the movie file contains embedded memory card data."),
		Category("Movie File")]
		public bool ContainsMemoryCards { get; set; }

		[Description("Indicates whether the movie contains an embedded cheat list."),
		Category("Movie File")]
		public bool ContainsCheats { get; set; }

		[Description("Indicates whether the movie uses hacks, such as \"SPU/SIO IRQ always enabled\""),
		Category("Configuration")]
		public bool ContainsHacks { get; set; }

		[Description("Indicates whether the input is stored in text (True) or binary (False) format."),
		Category("Movie File")]
		public bool TextFormat { get; set; }

		[Description("Whether a multitap was connected to port 1."),
		Category("Movie File")]
		public bool MultitapPort1 { get; set; }

		[Description("Whether a multitap was connected to port 2."),
		Category("Movie File")]
		public bool MultitapPort2 { get; set; }

		[Description("Indicates whether the movie uses the analog hack."),
		Category("Configuration")]
		public bool AnalogHack { get; set; }

		[Description("Indicates whether the movie uses the Parasite Eve 2, Vandal Hearts 1/2 fix."),
		Category("Configuration")]
		public bool ParasiteEveVandalHeartsFix { get; set; }
		
		[Description("The type of controller connected to port 1."),
		Category("Movie File")]
		public PSXControllerType ControllerTypePort1 { get; set; }

		[Description("The type of controller connected to port 2."),
		Category("Movie File")]
		public PSXControllerType ControllerTypePort2 { get; set; }

		[Description("The number of frames in the movie file."),
		Category("Movie File")]
		public uint FrameCount { get; set; }

		[Description("The number of times a state was loaded during recording of this movie file."),
		Category("Movie File")]
		public uint RerecordCount { get; set; }

		[Description("The name of the author."),
		Category("Movie File")]
		public string Author { get; set; }

		// Not editable
		public uint SavestateOffset;
		public uint MemoryCard1Offset;
		public uint MemoryCard2Offset;
		public uint CheatListOffset;
		public uint CDROMIDOffset;
		public uint ControllerDataOffset;
		public uint AuthorNameLength;
		
		[Description("The savestate the movie starts from, if it does not start from power on."),
		Category("Movie File")]
		public byte[] Savestate { get; set; }

		[Description("Data of the memory card in slot 1."),
		Category("Movie File")]
		public byte[] MemoryCard1 { get; set; }

		[Description("Data of the memory card in slot 2."),
		Category("Movie File")]
		public byte[] MemoryCard2 { get; set; }

		[Description("A possibly empty list of cheats used."),
		Category("Movie File")]
		public byte[] CheatList { get; set; }

		[Description("A list of the IDs of the CD-ROMs used in this movie."),
		Category("Game")]
		public byte[] CDROMIDs { get; set; }

		public void Read(FileStream stream)
		{
			var reader = new BinaryReader(stream);

			Signature       = reader.ReadBytes(4);
			MovieVersion    = reader.ReadUInt32();
			EmulatorVersion = reader.ReadUInt32();

			var flags = reader.ReadUInt16();
			StartsFromSavestate        = ((flags & 0x0002) > 0);
			PAL                        = ((flags & 0x0004) > 0);
			ContainsMemoryCards        = ((flags & 0x0008) > 0);
			ContainsCheats             = ((flags & 0x0010) > 0);
			ContainsHacks              = ((flags & 0x0020) > 0);
			TextFormat                 = ((flags & 0x0040) > 0);
			MultitapPort1              = ((flags & 0x0080) > 0);
			MultitapPort2              = ((flags & 0x0100) > 0);
			AnalogHack                 = ((flags & 0x0200) > 0);
			ParasiteEveVandalHeartsFix = ((flags & 0x0400) > 0);

			ControllerTypePort1 = (PSXControllerType)reader.ReadByte();
			ControllerTypePort2 = (PSXControllerType)reader.ReadByte();

			FrameCount = reader.ReadUInt32();
			RerecordCount = reader.ReadUInt32();

			SavestateOffset      = reader.ReadUInt32();
			MemoryCard1Offset    = reader.ReadUInt32();
			MemoryCard2Offset    = reader.ReadUInt32();
			CheatListOffset      = reader.ReadUInt32();
			CDROMIDOffset        = reader.ReadUInt32();
			ControllerDataOffset = reader.ReadUInt32();
			AuthorNameLength     = reader.ReadUInt32();

			var authorBytes = new List<byte>();
			byte b;

			while ((b = reader.ReadByte()) != 0)
				authorBytes.Add(b);

			Author = ASCIIEncoding.ASCII.GetString(authorBytes.ToArray());

			stream.Seek(SavestateOffset, SeekOrigin.Begin);
			Savestate = reader.ReadBytes((int)(MemoryCard1Offset - SavestateOffset));

			stream.Seek(MemoryCard1Offset, SeekOrigin.Begin);
			MemoryCard1 = reader.ReadBytes((int)(MemoryCard2Offset - MemoryCard1Offset));

			stream.Seek(MemoryCard2Offset, SeekOrigin.Begin);
			MemoryCard2 = reader.ReadBytes((int)(CheatListOffset - MemoryCard2Offset));

			stream.Seek(CheatListOffset, SeekOrigin.Begin);
			CheatList = reader.ReadBytes((int)(CDROMIDOffset - CheatListOffset));

			stream.Seek(CDROMIDOffset, SeekOrigin.Begin);
			CDROMIDs = reader.ReadBytes((int)(ControllerDataOffset - CDROMIDOffset));
		}

		public void Write(FileStream stream)
		{
			var writer = new BinaryWriter(stream);
			
			writer.Write(Signature);
			writer.Write(MovieVersion);
			writer.Write(EmulatorVersion);

			ushort flags = 0;
			flags |= (byte)(StartsFromSavestate        ? 0x0002 : 0x0000);
			flags |= (byte)(PAL                        ? 0x0004 : 0x0000);
			flags |= (byte)(ContainsMemoryCards        ? 0x0008 : 0x0000);
			flags |= (byte)(ContainsCheats             ? 0x0010 : 0x0000);
			flags |= (byte)(ContainsHacks              ? 0x0020 : 0x0000);
			flags |= (byte)(TextFormat                 ? 0x0040 : 0x0000);
			flags |= (byte)(MultitapPort1              ? 0x0080 : 0x0000);
			flags |= (byte)(MultitapPort2              ? 0x0100 : 0x0000);
			flags |= (byte)(AnalogHack                 ? 0x0200 : 0x0000);
			flags |= (byte)(ParasiteEveVandalHeartsFix ? 0x0400 : 0x0000);
			writer.Write(flags);

			writer.Write((byte)ControllerTypePort1);
			writer.Write((byte)ControllerTypePort2);

			writer.Write(FrameCount);
			writer.Write(RerecordCount);

			// Offsets and length indicator
			AuthorNameLength     = (uint)Author.Length;
			SavestateOffset      = 0x34 + AuthorNameLength;
			MemoryCard1Offset    = (uint)(SavestateOffset + Savestate.Length);
			MemoryCard2Offset    = (uint)(MemoryCard1Offset + MemoryCard1.Length);
			CheatListOffset      = (uint)(MemoryCard2Offset + MemoryCard2.Length);
			CDROMIDOffset        = (uint)(CheatListOffset + CheatList.Length);
			ControllerDataOffset = (uint)(CDROMIDOffset + CDROMIDs.Length);

			writer.Write(SavestateOffset);
			writer.Write(MemoryCard1Offset);
			writer.Write(MemoryCard2Offset);
			writer.Write(CheatListOffset);
			writer.Write(CDROMIDOffset);
			writer.Write(ControllerDataOffset);
			writer.Write(AuthorNameLength);

			writer.WriteASCII(Author);
			writer.Write(Savestate);
			writer.Write(MemoryCard1);
			writer.Write(MemoryCard2);
			writer.Write(CheatList);
			writer.Write(CDROMIDs);
		}
	}
}
