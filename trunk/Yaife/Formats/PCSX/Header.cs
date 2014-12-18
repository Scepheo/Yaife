using System.IO;
using Yaife.Utilities;

namespace Yaife.Formats.PCSX
{
	public class Header
	{
		public byte[] Signature { get; set; }
		public uint MovieVersion { get; set; }
		public uint EmulatorVersion { get; set; }
		public bool StartsFromSavestate { get; set; }
		public bool PAL { get; set; }
		public bool ContainsMemoryCards { get; set; }
		public bool ContainsCheats { get; set; }
		public bool ContainsHacks { get; set; }
		public PSXControllerType ControllerTypePort1 { get; set; }
		public PSXControllerType ControllerTypePort2 { get; set; }
		public uint FrameCount { get; set; }
		public uint RerecordCount { get; set; }
		public string Author { get; set; }

		// Not editable
		public uint SavestateOffset;
		public uint MemoryCard1Offset;
		public uint MemoryCard2Offset;
		public uint CheatListOffset;
		public uint CDROMIDOffset;
		public uint ControllerDataOffset;
		public uint AuthorNameLength;

		// More stuff
		public byte[] Savestate { get; set; }
		public byte[] MemoryCard1 { get; set; }
		public byte[] MemoryCard2 { get; set; }
		public byte[] CheatList { get; set; }
		public byte[] CDROMIDs { get; set; }

		public void Read(FileStream stream)
		{
			var reader = new BinaryReader(stream);

			Signature       = reader.ReadBytes(4);
			MovieVersion    = reader.ReadUInt32();
			EmulatorVersion = reader.ReadUInt32();

			var flags           = reader.ReadByte();
			StartsFromSavestate = ((flags & 0x02) > 0);
			PAL                 = ((flags & 0x04) > 0);
			ContainsMemoryCards = ((flags & 0x08) > 0);
			ContainsCheats      = ((flags & 0x10) > 0);
			ContainsHacks       = ((flags & 0x20) > 0);
			
			// Skip reserved flags
			reader.ReadByte();

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

			Author = reader.ReadASCII((int)AuthorNameLength);

			Savestate   = reader.ReadBytes((int)(MemoryCard1Offset    - SavestateOffset));
			MemoryCard1 = reader.ReadBytes((int)(MemoryCard2Offset    - MemoryCard1Offset));
			MemoryCard2 = reader.ReadBytes((int)(CheatListOffset      - MemoryCard2Offset));
			CheatList   = reader.ReadBytes((int)(CDROMIDOffset        - CheatListOffset));
			CDROMIDs    = reader.ReadBytes((int)(ControllerDataOffset - CDROMIDOffset));
		}

		public void Write(FileStream stream)
		{
			var writer = new BinaryWriter(stream);
			
			writer.Write(Signature);
			writer.Write(MovieVersion);
			writer.Write(EmulatorVersion);

			byte flags = 0;
			flags |= (byte)(StartsFromSavestate ? 0x02 : 0x00);
			flags |= (byte)(PAL                 ? 0x04 : 0x00);
			flags |= (byte)(ContainsMemoryCards ? 0x08 : 0x00);
			flags |= (byte)(ContainsCheats      ? 0x10 : 0x00);
			flags |= (byte)(ContainsHacks       ? 0x20 : 0x00);
			writer.Write(flags);

			// Write unused flags
			writer.Write((byte)0);

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
