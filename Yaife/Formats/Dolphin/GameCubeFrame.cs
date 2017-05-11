using System;

namespace Yaife.Formats.Dolphin
{
    [Flags]
    public enum GcButton : short
    {
        None  = 0x0000,
        Left  = 0x0001,
        Right = 0x0002,
        Down  = 0x0004,
        Up    = 0x0008,
        Z     = 0x0010,
        R     = 0x0020,
        L     = 0x0040,
        A     = 0x0100,
        B     = 0x0200,
        X     = 0x0400,
        Y     = 0x0800,
        Start = 0x1000,
        Disc  = 0x2000
    }

    public class GameCubeFrame : IFrame
    {
        private GcButton _button;
        private byte _triggerL, _triggerR;
        private sbyte _analogX, _analogY;
        private sbyte _cStickX, _cStickY;

        public GameCubeFrame() : this(new byte[8]) {}
        
        public GameCubeFrame(byte[] data)
        {
            _button = (GcButton)(data[0] | (data[1] << 8));

            _triggerL = data[2];
            _triggerR = data[3];
            _analogX  = (sbyte)(data[4] - 128);
            _analogY  = (sbyte)(data[5] - 128);
            _cStickX  = (sbyte)(data[6] - 128);
            _cStickY  = (sbyte)(data[7] - 128);
        }

        public object[] ToStrings()
        {
            var result = new object[5];

            result[0] = _button.ToString();
            result[1] = $"{_triggerL:d3}";
            result[2] = $"{_triggerR:d3}";
            result[3] = $"{_analogX,4:d3},{_analogY,4:d3}";
            result[4] = $"{_cStickX,4:d3},{_cStickY,4:d3}";

            return result;
        }

        public void Parse(string[] str)
        {
            // Parse buttons first, accepting empty strings as None
            if (string.IsNullOrWhiteSpace(str[0]))
                _button = GcButton.None;
            else
                _button = (GcButton)Enum.Parse(typeof(GcButton), str[0]);

            // Parse triggers
            _triggerL = byte.Parse(str[1]);
            _triggerR = byte.Parse(str[2]);

            // Parse analogs
            var analogSplit = str[3].Split(',');
            _analogX = sbyte.Parse(analogSplit[0]);
            _analogY = sbyte.Parse(analogSplit[1]);

            // Parse C-Stick
            var cStickSplit = str[4].Split(',');
            _cStickX = sbyte.Parse(cStickSplit[0]);
            _cStickY = sbyte.Parse(cStickSplit[1]);
        }

        public byte[] GetBytes()
        {
            var data = new byte[8];
            var int16 = (short)_button;

            data[0] = (byte)(int16 & 0xFF);
            data[1] = (byte)(int16 >> 8);
            data[2] = _triggerL;
            data[3] = _triggerR;
            data[4] = (byte)(_analogX + 128);
            data[5] = (byte)(_analogY + 128);
            data[6] = (byte)(_cStickX + 128);
            data[7] = (byte)(_cStickY + 128);

            return data;
        }

        public bool IsEmpty()
        {
            return
            _button == GcButton.None &&
            _triggerL != 0 && _triggerR != 0 &&
            _analogX != 0 && _analogY != 0 &&
            _cStickX != 0 && _cStickY != 0;
        }
    }
}
