using System;
using System.ComponentModel;
using System.IO;
using Yaife.Editors;
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
        public int Uid { get; set; }

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
        public bool UseBios { get; set; }

        [Description("Whether the BIOS intro was skipped (True) or included in the movie file (False). Only applies to GBA and use of an external BIOS file."),
        Category("Configuration")]
        public bool SkipBios { get; set; }

        [Description("Whether the real-time clock feature was used for this movie."),
        Category("Configuration")]
        public bool EnableRtc { get; set; }

        [Description("If True, the movie was made with Null Input Kludge on (does not apply to GBA)."),
        Category("Configuration")]
        public bool InputHack { get; set; }

        [Description("Whether the movie uses the new GBA timing (GBA only)."),
        Category("Configuration")]
        public bool ReduceLag { get; set; }

        [Description("Whether the movie uses the new GBC HDMA5 timing (GBC only)."),
        Category("Configuration")]
        public bool Hdma5TimingFix { get; set; }

        [Description("Whether the movie was recorded with Echo RAM Fix (does not apply to GBA)."),
        Category("Configuration")]
        public bool EchoRamFix { get; set; }

        [Description("Whether the movie was recorded with SRAM Init Fix (GBA only)."),
        Category("Configuration")]
        public bool SramInitFix { get; set; }

        [Description("The save type used by the emulator for this movie."),
        Category("Configuration")]
        public SaveType WinSaveType { get; set; }

        [Description("The size of the flash memory used by the emulator for this movie."),
        Category("Configuration")]
        public FlashSize WinFlashSize { get; set; }

        [Description("The emulator type used for this movie."),
        Category("Configuration")]
        public EmulatorType GbEmulatorType { get; set; }

        [Description("Internal game name of the ROM (12 characters)."),
        Category("Game")]
        public string GameName { get; set; }

        [Description("The minor version number of the movie file type, latest is 1."),
        Category("Movie File")]
        public byte MinorVersion { get; set; }

        [Description("Internal CRC of the ROM."),
        Category("Game")]
        public byte InternalRomChecksum { get; set; }

        [Description("A 16-bit CRC of the BIOS if GBA, otherwise the internal CRC of the ROM."),
        Category("Game")]
        public ushort RomChecksum { get; set; }

        [Description("The Game Code of the ROM if GBA, or the Unit Code otherwise."),
        Category("Game")]
        public uint GameCode { get; set; }

        // Not editable
        public uint SramOffset;
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
            Uid                  = reader.ReadInt32();
            FrameCount           = reader.ReadUInt32();
            RerecordCount        = reader.ReadUInt32();
            StartFrom            = (StartFrom)reader.ReadByte();
            ConnectedControllers = (ConnectedControllers)reader.ReadByte();
            System               = (System)reader.ReadByte();
            
            var flags      = reader.ReadByte();
            UseBios        = (flags & 0x01) > 0;
            SkipBios       = (flags & 0x02) > 0;
            EnableRtc      = (flags & 0x04) > 0;
            InputHack      = (flags & 0x08) > 0;
            ReduceLag      = (flags & 0x10) > 0;
            Hdma5TimingFix = (flags & 0x20) > 0;
            EchoRamFix     = (flags & 0x40) > 0;
            SramInitFix    = (flags & 0x80) > 0;

            WinSaveType     = (SaveType)reader.ReadUInt32();
            WinFlashSize    = (FlashSize)reader.ReadUInt32();
            GbEmulatorType  = (EmulatorType)reader.ReadUInt32();
            GameName        = reader.ReadASCII(12);
            MinorVersion    = reader.ReadByte();
            InternalRomChecksum = reader.ReadByte();
            RomChecksum     = reader.ReadUInt16();
            GameCode        = reader.ReadUInt32();

            SramOffset       = reader.ReadUInt32();
            ControllerOffset = reader.ReadUInt32();

            Author      = reader.ReadASCII(64);
            Description = reader.ReadASCII(128);

            if (StartFrom != StartFrom.PowerOn)
                StartSave = reader.ReadBytes((int)(SramOffset - ControllerOffset));
        }

        public void Write(FileStream stream)
        {
            var writer = new BinaryWriter(stream);

            writer.Write(Signature);
            writer.Write(MajorVersion);
            writer.Write(Uid);
            writer.Write(FrameCount);
            writer.Write(RerecordCount);
            writer.Write((byte)StartFrom);
            writer.Write((byte)ConnectedControllers);
            writer.Write((byte)System);

            byte flags = 0;
            flags |= (byte)(UseBios        ? 0x01 : 0x00);
            flags |= (byte)(SkipBios       ? 0x02 : 0x00);
            flags |= (byte)(EnableRtc      ? 0x04 : 0x00);
            flags |= (byte)(InputHack      ? 0x08 : 0x00);
            flags |= (byte)(ReduceLag      ? 0x10 : 0x00);
            flags |= (byte)(Hdma5TimingFix ? 0x20 : 0x00);
            flags |= (byte)(EchoRamFix     ? 0x40 : 0x00);
            flags |= (byte)(SramInitFix    ? 0x80 : 0x00);
            writer.Write(flags);

            writer.Write((uint)WinSaveType);
            writer.Write((uint)WinFlashSize);
            writer.Write((uint)GbEmulatorType);
            writer.WriteASCII(GameName, 12);
            writer.Write(MinorVersion);
            writer.Write(InternalRomChecksum);
            writer.Write(RomChecksum);
            writer.Write(GameCode);

            if (StartFrom == StartFrom.PowerOn)
            {
                SramOffset       = 0;
                ControllerOffset = 0x100;
            }
            else
            {
                SramOffset       = 0x100;
                ControllerOffset = (uint)(SramOffset + StartSave.Length);
            }

            writer.Write(SramOffset);
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
        Sram      = 0x02
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
        Eeprom       = 1,
        Sram         = 2,
        Flash        = 3,
        EepromSensor = 4,
        None         = 5
    }

    public enum FlashSize : uint
    {
        Flash512K = 0x10000,
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
