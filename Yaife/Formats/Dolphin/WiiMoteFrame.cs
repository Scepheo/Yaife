using System;
using System.Collections.Generic;
using System.IO;
using Yaife.Utilities;

namespace Yaife.Formats.Dolphin
{
    public class WiiMoteFrame : IFrame
    {
        private PacketType _packetType = PacketType.Buttons;
        private WiiMoteButtons _buttons = WiiMoteButtons.None;
        private byte[] _data = new byte[0];
        
        public WiiMoteFrame() {}

        public WiiMoteFrame(BinaryReader stream)
        {
            // We can skip length, as this is implicit
            stream.ReadByte();

            if (stream.ReadByte() != 0xA1)
                throw new Exception("Input stream signature does not match.");

            _packetType = (PacketType)stream.ReadByte();

            if (_packetType != PacketType.Extension21)
                _buttons = (WiiMoteButtons)stream.ReadUInt16();

            _data = stream.ReadBytes(GetDataSize(_packetType));
        }

        public object[] ToStrings()
        {
            var result = new object[3];

            result[0] = _packetType.ToString();

            if (_packetType == PacketType.Extension21 || _buttons == WiiMoteButtons.None)
                result[1] = "";
            else
                result[1] = _buttons.ToString();

            result[2] = _data.ToHexString();

            return result;
        }

        public void Parse(string[] str)
        {
            // Parse packet type
            _packetType = (PacketType)Enum.Parse(typeof(PacketType), str[0]);

            // Parse buttons, accepting empty strings as None
            if (_packetType == PacketType.Extension21 || string.IsNullOrWhiteSpace(str[1]))
                _buttons = WiiMoteButtons.None;
            else
                _buttons = (WiiMoteButtons)Enum.Parse(typeof(WiiMoteButtons), str[1]);

            // Parse rest of data
            _data = HexString.Parse(str[2]);
        }

        public byte[] GetBytes()
        {
            var list = new List<byte> { 0xA1, (byte)_packetType };
            
            if (_packetType != PacketType.Extension21)
            {
                var uint16 = (ushort)_buttons;
                list.Add((byte)(uint16 & 0xFF));
                list.Add((byte)(uint16 >> 8));
            }

            list.AddRange(_data);
            list.Insert(0, (byte)list.Count);

            return list.ToArray();
        }

        public bool IsEmpty()
        {
            // I'm pretty sure the concept of an empty wiimote packet isn't useful.
            return false;
        }

        public int GetDataSize(PacketType type)
        {
            switch (type)
            {
                default:
                    return 0;
                case PacketType.Acknowledge:
                    return 2;
                case PacketType.Accelerometer:
                    return 3;
                case PacketType.StatusInformation:
                    return 4;
                case PacketType.Extension8:
                    return 8;
                case PacketType.AccelerometerIr:
                    return 15;
                case PacketType.ReadMemory:
                case PacketType.Extension19:
                case PacketType.AccelerometerExtension16:
                case PacketType.IrExtension9:
                case PacketType.AccelerometerIrExtension6:
                case PacketType.AccelerometerIrA:
                case PacketType.AccelerometerIrB:
                    return 19;
                case PacketType.Extension21:
                    return 21;
            }
        }
    }

    public enum PacketType : byte
    {
        StatusInformation            = 0x20,
        ReadMemory                   = 0x21,
        Acknowledge                  = 0x22,
        Buttons                      = 0x30,
        Accelerometer                = 0x31,
        Extension8                  = 0x32,
        AccelerometerIr             = 0x33,
        Extension19                 = 0x34,
        AccelerometerExtension16   = 0x35,
        IrExtension9               = 0x36,
        AccelerometerIrExtension6 = 0x37,
        Extension21                 = 0x3D,
        AccelerometerIrA           = 0x3E,
        AccelerometerIrB           = 0x3F
    }

    [Flags]
    public enum WiiMoteButtons : ushort
    {
        None  = 0x0000,
        Left  = 0x0001,
        Right = 0x0002,
        Down  = 0x0004,
        Up    = 0x0008,
        Plus  = 0x0010,
        Two   = 0x0100,
        One   = 0x0200,
        B     = 0x0400,
        A     = 0x0800,
        Minus = 0x1000,
        Home  = 0x8000
    }

    public enum WiiMoteError : byte
    {
        Success  = 0x00,
        Error    = 0x03,
        Unknown1 = 0x04,
        Unknown2 = 0x05,
        Unknown3 = 0x08
    }

    public enum WiiMoteReadError : byte
    {
        Success     = 0x00,
        WriteOnly   = 0x07,
        NonExistent = 0x08
    }
}