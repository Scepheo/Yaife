using System.ComponentModel;
using System.IO;
using Yaife.Editors;
using Yaife.Utilities;

namespace Yaife.Formats.PCSX
{
    public class Header
    {
        [Description("Unique file type signature, should be 50 58 4D 20."),
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
        public bool Pal { get; set; }

        [Description("Indicates whether the movie file contains embedded memory card data."),
        Category("Movie File")]
        public bool ContainsMemoryCards { get; set; }

        [Description("Indicates whether the movie contains an embedded cheat list."),
        Category("Movie File")]
        public bool ContainsCheats { get; set; }

        [Description("Indicates whether the movie uses hacks, such as \"SPU/SIO IRQ always enabled\""),
        Category("Configuration")]
        public bool ContainsHacks { get; set; }

        [Description("The type of controller connected to port 1."),
        Category("Movie File")]
        public PsxControllerType ControllerTypePort1 { get; set; }

        [Description("The type of controller connected to port 2."),
        Category("Movie File")]
        public PsxControllerType ControllerTypePort2 { get; set; }

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
        public uint CdromidOffset;
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
        public byte[] CdromiDs { get; set; }

        public void Read(FileStream stream)
        {
            var reader = new BinaryReader(stream);

            Signature       = reader.ReadBytes(4);
            MovieVersion    = reader.ReadUInt32();
            EmulatorVersion = reader.ReadUInt32();

            var flags           = reader.ReadByte();
            StartsFromSavestate = (flags & 0x02) > 0;
            Pal                 = (flags & 0x04) > 0;
            ContainsMemoryCards = (flags & 0x08) > 0;
            ContainsCheats      = (flags & 0x10) > 0;
            ContainsHacks       = (flags & 0x20) > 0;
            
            // Skip reserved flags
            reader.ReadByte();

            ControllerTypePort1 = (PsxControllerType)reader.ReadByte();
            ControllerTypePort2 = (PsxControllerType)reader.ReadByte();

            FrameCount = reader.ReadUInt32();
            RerecordCount = reader.ReadUInt32();

            SavestateOffset      = reader.ReadUInt32();
            MemoryCard1Offset    = reader.ReadUInt32();
            MemoryCard2Offset    = reader.ReadUInt32();
            CheatListOffset      = reader.ReadUInt32();
            CdromidOffset        = reader.ReadUInt32();
            ControllerDataOffset = reader.ReadUInt32();
            AuthorNameLength     = reader.ReadUInt32();

            Author = reader.ReadASCII((int)AuthorNameLength);

            Savestate   = reader.ReadBytes((int)(MemoryCard1Offset    - SavestateOffset));
            MemoryCard1 = reader.ReadBytes((int)(MemoryCard2Offset    - MemoryCard1Offset));
            MemoryCard2 = reader.ReadBytes((int)(CheatListOffset      - MemoryCard2Offset));
            CheatList   = reader.ReadBytes((int)(CdromidOffset        - CheatListOffset));
            CdromiDs    = reader.ReadBytes((int)(ControllerDataOffset - CdromidOffset));
        }

        public void Write(FileStream stream)
        {
            var writer = new BinaryWriter(stream);
            
            writer.Write(Signature);
            writer.Write(MovieVersion);
            writer.Write(EmulatorVersion);

            byte flags = 0;
            flags |= (byte)(StartsFromSavestate ? 0x02 : 0x00);
            flags |= (byte)(Pal                 ? 0x04 : 0x00);
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
            CdromidOffset        = (uint)(CheatListOffset + CheatList.Length);
            ControllerDataOffset = (uint)(CdromidOffset + CdromiDs.Length);

            writer.Write(SavestateOffset);
            writer.Write(MemoryCard1Offset);
            writer.Write(MemoryCard2Offset);
            writer.Write(CheatListOffset);
            writer.Write(CdromidOffset);
            writer.Write(ControllerDataOffset);
            writer.Write(AuthorNameLength);

            writer.WriteASCII(Author);
            writer.Write(Savestate);
            writer.Write(MemoryCard1);
            writer.Write(MemoryCard2);
            writer.Write(CheatList);
            writer.Write(CdromiDs);
        }
    }
}
