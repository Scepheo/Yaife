using System;

namespace Yaife.Formats.VisualBoyAdvance
{
    public class Frame : IFrame
    {
        public GameboyButtons[] Controllers;

        public Frame() : this(new byte[2])
        { }

        public Frame(byte[] data)
        {
            var count = data.Length / 2;
            Controllers = new GameboyButtons[count];

            for (var i = 0; i < count; i++)
            {
                var int16 = (ushort)(data[i * 2] | (data[i * 2 + 1] << 8));
                Controllers[i] = (GameboyButtons)int16;
            }
        }

        public object[] ToStrings()
        {
            var result = new object[Controllers.Length];

            for (var i = 0; i < Controllers.Length; i++)
                result[i] = Controllers[i].ToString();

            return result;
        }

        public void Parse(string[] strings)
        {
            Controllers = new GameboyButtons[strings.Length];

            for (var i = 0; i < strings.Length; i++)
                Controllers[i] = (GameboyButtons)Enum.Parse(typeof(GameboyButtons), strings[i]);
        }

        public byte[] GetBytes()
        {
            var result = new byte[2 * Controllers.Length];

            for (var i = 0; i < Controllers.Length; i++)
            {
                var int16 = (ushort)Controllers[i];
                result[i * 2] = (byte)(int16 & 0x00FF);
                result[i * 2 + 1] = (byte)(int16 >> 8);
            }

            return result;
        }
    }

    [Flags]
    public enum GameboyButtons : ushort
    {
        Empty       = 0x0000,
        A           = 0x0001,
        B           = 0x0002,
        Select      = 0x0004,
        Start       = 0x0008,
        Right       = 0x0010,
        Left        = 0x0020,
        Up          = 0x0040,
        Down        = 0x0080,
        R           = 0x0100,
        L           = 0x0200,
        OldReset    = 0x0400,
        Reset       = 0x0800,
        MotionLeft  = 0x1000,
        MotionRight = 0x2000,
        MotionDown  = 0x4000,
        MotionUp    = 0x8000
    }
}
