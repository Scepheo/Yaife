using System;

namespace Yaife.Formats.PCSX
{
    public abstract class PsxController
    {
        public abstract void Parse(string[] str);
        public abstract string[] ToStrings();
        public abstract byte[] GetBytes();
        public abstract bool IsEmpty();

        public static PsxController GetController(PsxControllerType type, byte[] data)
        {
            switch (type)
            {
                case PsxControllerType.StandardController:
                    return new StandardController(data);
                case PsxControllerType.Mouse:
                    return new Mouse(data);
                case PsxControllerType.Joystick:
                case PsxControllerType.AnalogController:
                    return new AnalogController(data);
                default:
                    return new StandardController(new byte[2]);
            }
        }

        public static string[] GetHeaders(PsxControllerType type)
        {
            switch (type)
            {
                case PsxControllerType.Mouse:
                    return new[] { "Buttons", "Movement" };
                case PsxControllerType.Joystick:
                case PsxControllerType.AnalogController:
                    return new[] { "Buttons", "Left", "Right" };
                default:
                    return new[] { "Buttons" };
            }
        }
    }

    public enum PsxControllerType : byte
    {
        Mouse = 1,
        Negcon = 2,
        KonamiGun = 3,
        StandardController = 4,
        Joystick = 5,
        NamcoGun = 6,
        AnalogController = 7
    }

    [Flags]
    public enum Buttons : ushort
    {
        None     = 0x0000,
        Select   = 0x0001,
        Unknown1 = 0x0002,
        Unknown2 = 0x0004,
        Start    = 0x0008,
        Up       = 0x0010,
        Right    = 0x0020,
        Down     = 0x0040,
        Left     = 0x0080,
        L2       = 0x0100,
        R2       = 0x0200,
        L1       = 0x0400,
        R1       = 0x0800,
        Triangle = 0x1000,
        Circle   = 0x2000,
        Cross    = 0x4000,
        Square   = 0x8000
    }

    public class StandardController : PsxController
    {
        private Buttons _buttons;

        public StandardController(byte[] data)
        {
            var x = (ushort)(data[0] | (data[1] << 8));
            _buttons = (Buttons)x;
        }

        public override void Parse(string[] str)
        {
            _buttons = (Buttons)Enum.Parse(typeof(Buttons), str[0]);
        }

        public override string[] ToStrings()
        {
            return new[] { _buttons.ToString() };
        }

        public override byte[] GetBytes()
        {
            var result = new byte[2];

            var int16 = (ushort)_buttons;
            result[0] = (byte)(int16 & 0xFF);
            result[1] = (byte)(int16 >> 8);

            return result;
        }

        public override bool IsEmpty()
        {
            return _buttons == Buttons.None;
        }
    }

    public class Mouse : PsxController
    {
        private Buttons _buttons;
        private sbyte _dX, _dY;

        public Mouse(byte[] data)
        {
            var x = (ushort)(data[0] | (data[1] << 8));
            _buttons = (Buttons)x;
            _dX = (sbyte)(data[2] - 128);
            _dY = (sbyte)(data[3] - 128);
        }

        public override void Parse(string[] str)
        {
            _buttons = (Buttons)Enum.Parse(typeof(Buttons), str[0]);
            
            var movementSplit = str[1].Split(',');
            _dX = sbyte.Parse(movementSplit[0]);
            _dY = sbyte.Parse(movementSplit[1]);
        }

        public override string[] ToStrings()
        {
            var result = new string[2];

            result[0] = _buttons.ToString();
            result[1] = $"{_dX,4:d3},{_dY,4:d3}";

            return result;
        }

        public override byte[] GetBytes()
        {
            var result = new byte[4];

            var int16 = (ushort)_buttons;
            result[0] = (byte)(int16 & 0xFF);
            result[1] = (byte)(int16 >> 8);
            result[2] = (byte)(_dX + 128);
            result[3] = (byte)(_dY + 128);

            return result;
        }

        public override bool IsEmpty()
        {
            return _buttons == Buttons.None
                   && _dX == 0 && _dY == 0;
        }
    }

    public class AnalogController : PsxController
    {
        private Buttons _buttons;
        private sbyte _leftX, _leftY, _rightX, _rightY;

        public AnalogController(byte[] data)
        {
            var x = (ushort)(data[0] | (data[1] << 8));
            _buttons = (Buttons)x;
            _leftX = (sbyte)(data[2] - 128);
            _leftY = (sbyte)(data[3] - 128);
            _rightX = (sbyte)(data[4] - 128);
            _rightY = (sbyte)(data[5] - 128);
        }

        public override void Parse(string[] str)
        {
            _buttons = (Buttons)Enum.Parse(typeof(Buttons), str[0]);

            var leftSplit = str[1].Split(',');
            _leftX = sbyte.Parse(leftSplit[0]);
            _leftY = sbyte.Parse(leftSplit[1]);

            var rightSplit = str[2].Split(',');
            _rightX = sbyte.Parse(rightSplit[0]);
            _rightY = sbyte.Parse(rightSplit[1]);
        }

        public override string[] ToStrings()
        {
            var result = new string[3];

            result[0] = _buttons.ToString();
            result[1] = $"{_leftX,4:d3},{_leftY,4:d3}";
            result[2] = $"{_rightX,4:d3},{_rightY,4:d3}";

            return result;
        }

        public override byte[] GetBytes()
        {
            var result = new byte[6];

            var int16 = (ushort)_buttons;
            result[0] = (byte)(int16 & 0xFF);
            result[1] = (byte)(int16 >> 8);
            result[2] = (byte)(_leftX + 128);
            result[3] = (byte)(_leftY + 128);
            result[4] = (byte)(_rightX + 128);
            result[5] = (byte)(_rightY + 128);

            return result;
        }

        public override bool IsEmpty()
        {
            return _buttons == Buttons.None
                   && _leftX == 0 && _leftY == 0
                   && _rightX == 0 && _rightY == 0;
        }
    }
}
