using System.Collections.Generic;
using System.IO;
using System.Text;
using Yaife.Utilities;

namespace Yaife.Formats.PSXjin
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
		public bool TextFormat { get; set; }
		public bool MultitapPort1 { get; set; }
		public bool MultitapPort2 { get; set; }
		public bool AnalogHack { get; set; }
		public bool ParasiteEveVandalHeartsFix { get; set; }
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
