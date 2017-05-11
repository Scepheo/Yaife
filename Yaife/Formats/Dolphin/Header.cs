using System;
using System.ComponentModel;
using System.IO;
using Yaife.Editors;
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
        public string GameId { get; set; }

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
        public ulong UniqueId { get; set; }

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
        public byte[] Md5 { get; set; }

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
        public bool Dsphle { get; set; }

        [Description(""),
        Category("Configuration")]
        public bool FastDiscSpeed { get; set; }

        [Description("The core used to emulate the CPU."),
        Category("Configuration")]
        public CpuCore CpuCore { get; set; }

        [Description(""),
        Category("Configuration")]
        public bool EfbAccessEnable { get; set; }

        [Description(""),
        Category("Configuration")]
        public bool EfbCopyEnable { get; set; }

        [Description(""),
        Category("Configuration")]
        public bool CopyEfbToTexture { get; set; }

        [Description(""),
        Category("Configuration")]
        public bool EfbCopyCacheEnable { get; set; }

        [Description(""),
        Category("Configuration")]
        public bool EfbEmulateFormatChanges { get; set; }

        [Description("Whether an external frame buffer is used."),
        Category("Configuration")]
        public bool UseXfb { get; set; }

        [Description("If an external frame buffer is used, whether to use a real one (True) or a virtual one (False)."),
        Category("Configuration")]
        public bool UseRealXfb { get; set; }

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
        public bool SyncGpu { get; set; }

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
        public uint DsPiromHash { get; set; }

        [Description("")]
        public uint DsPcoefHash { get; set; }

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
            GameId                  = reader.ReadUtf8(6);
            IsWii                   = reader.ReadBoolean();
            Controllers             = (Controller)reader.ReadByte();
            StartsFromSaveState     = reader.ReadBoolean();
            FrameCount              = reader.ReadUInt64();
            InputCount              = reader.ReadUInt64();
            LagCount                = reader.ReadUInt64();
            UniqueId                = reader.ReadUInt64();
            RerecordCount           = reader.ReadUInt32();
            Author                  = reader.ReadUtf8(32);
            VideoBackend            = reader.ReadUtf8(16);
            AudioEmulator           = reader.ReadUtf8(16);
            Md5                     = reader.ReadBytes(16);
            SystemStartTime         = reader.ReadUInt64();
            SaveConfig              = reader.ReadBoolean();
            SkipIdle                = reader.ReadBoolean();
            DualCore                = reader.ReadBoolean();
            Progressive             = reader.ReadBoolean();
            Dsphle                  = reader.ReadBoolean();
            FastDiscSpeed           = reader.ReadBoolean();
            CpuCore                 = (CpuCore)reader.ReadByte();
            EfbAccessEnable         = reader.ReadBoolean();
            EfbCopyEnable           = reader.ReadBoolean();
            CopyEfbToTexture        = reader.ReadBoolean();
            EfbCopyCacheEnable      = reader.ReadBoolean();
            EfbEmulateFormatChanges = reader.ReadBoolean();
            UseXfb                  = reader.ReadBoolean();
            UseRealXfb              = reader.ReadBoolean();
            MemoryCards             = (MemoryCard)reader.ReadByte();
            ClearSave               = reader.ReadBoolean();
            Bongos                  = (Controller)reader.ReadByte();
            SyncGpu                 = reader.ReadBoolean();
            NetPlay                 = reader.ReadBoolean();
            ReservedConfiguration   = reader.ReadBytes(13);
            DiscChangeName          = reader.ReadUtf8(40);
            GitRevisionHash         = reader.ReadBytes(20);
            DsPiromHash             = reader.ReadUInt32();
            DsPcoefHash             = reader.ReadUInt32();
            TickCount               = reader.ReadUInt64();
            ReservedPadding         = reader.ReadBytes(11);
        }

        public void Write(FileStream stream)
        {
            var writer = new BinaryWriter(stream);

            writer.Write(FileType);
            writer.WriteUtf8(GameId, 6);
            writer.Write(IsWii);
            writer.Write((byte)Controllers);
            writer.Write(StartsFromSaveState);
            writer.Write(FrameCount);
            writer.Write(InputCount);
            writer.Write(LagCount);
            writer.Write(UniqueId);
            writer.Write(RerecordCount);
            writer.WriteUtf8(Author, 32);
            writer.WriteUtf8(VideoBackend, 16);
            writer.WriteUtf8(AudioEmulator, 16);
            writer.Write(Md5);
            writer.Write(SystemStartTime);
            writer.Write(SaveConfig);
            writer.Write(SkipIdle);
            writer.Write(DualCore);
            writer.Write(Progressive);
            writer.Write(Dsphle);
            writer.Write(FastDiscSpeed);
            writer.Write((byte)CpuCore);
            writer.Write(EfbAccessEnable);
            writer.Write(EfbCopyEnable);
            writer.Write(CopyEfbToTexture);
            writer.Write(EfbCopyCacheEnable);
            writer.Write(EfbEmulateFormatChanges);
            writer.Write(UseXfb);
            writer.Write(UseRealXfb);
            writer.Write((byte)MemoryCards);
            writer.Write(ClearSave);
            writer.Write((byte)Bongos);
            writer.Write(SyncGpu);
            writer.Write(NetPlay);
            writer.Write(ReservedConfiguration);
            writer.WriteUtf8(DiscChangeName, 40);
            writer.Write(GitRevisionHash);
            writer.Write(DsPiromHash);
            writer.Write(DsPcoefHash);
            writer.Write(TickCount);
            writer.Write(ReservedPadding);
        }
    }

    public enum CpuCore : byte
    {
        Interpreter = 0,
        Jit = 1,
        Jitil = 2
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
        GcController1 = 0x01,
        GcController2 = 0x02,
        GcController3 = 0x04,
        GcController4 = 0x08,
        WiiMote1      = 0x10,
        WiiMote2      = 0x20,
        WiiMote3      = 0x40,
        WiiMote4      = 0x80
    }
}
