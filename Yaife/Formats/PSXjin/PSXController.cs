using System;
using System.Text;

namespace Yaife.Formats.PSXjin
{
    public abstract class PsxController
    {
        public abstract void Parse(string[] str);
        public abstract string[] ToStrings();
        public abstract byte[] GetBytes();
        public abstract string GetText();
        public abstract bool IsEmpty();

        public static PsxController GetController(PsxControllerType type, byte[] data, bool text)
        {
            switch (type)
            {
                case PsxControllerType.StandardController:
                    return new StandardController(data, text);
                case PsxControllerType.Mouse:
                    return new Mouse(data, text);
                case PsxControllerType.Joystick:
                case PsxControllerType.AnalogController:
                    return new AnalogController(data, text);
                default:
                    return new StandardController(new byte[2], false);
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

    [Flags]
    public enum MouseButtons : ushort
    {
        None  = 0x0000,
        Left  = 0x0001,
        Right = 0x0002
    }

    public class StandardController : PsxController
    {
        private Buttons _buttons;

        public StandardController(byte[] data, bool text)
        {
            if (text)
            {
                var str = Encoding.ASCII.GetString(data);
                ushort flags = 0;

                for (var i = 0; i < 13; i++)
                {
                    flags <<= 1;

                    if (str[i] != '.')
                        flags |= 1;
                }

                // Read select
                flags <<= 3;

                if (str[13] != '.')
                    flags |= 1;

                _buttons = (Buttons)flags;
            }
            else
            {
                var int16 = (ushort)(data[0] | (data[1] << 8));
                _buttons = (Buttons)int16;
            }
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

        public override string GetText()
        {
            var result = "..............".ToCharArray();
            var codes  = "#X0^1234LDRUSs".ToCharArray();

            // Select is weird, so we do that separately
            for (var i = 0; i < 13; i++)
            {
                var flag = (Buttons)(0x8000 >> i);

                if (_buttons.HasFlag(flag))
                    result[i] = codes[i];
            }

            if (_buttons.HasFlag(Buttons.Select))
                result[13] = codes[13];

            return new string(result);
        }

        public override bool IsEmpty()
        {
            return _buttons == Buttons.None;
        }
    }

    public class Mouse : PsxController
    {
        private MouseButtons _buttons;
        private sbyte _dX, _dY;

        public Mouse(byte[] data, bool text)
        {
            if (text)
            {
                var str = Encoding.ASCII.GetString(data);

                _buttons = MouseButtons.None;

                if (str[0] != '.')
                    _buttons |= MouseButtons.Left;
                if (str[1] != '.')
                    _buttons |= MouseButtons.Right;

                _dX = sbyte.Parse(str.Substring(3, 3));
                _dY = sbyte.Parse(str.Substring(7, 3));
            }
            else
            {
                var x = (ushort)(data[0] | (data[1] << 8));
                _buttons = (MouseButtons)x;
                _dX = (sbyte)(data[2] - 128);
                _dY = (sbyte)(data[3] - 128);
            }
        }

        public override void Parse(string[] str)
        {
            _buttons = (MouseButtons)Enum.Parse(typeof(MouseButtons), str[0]);
            
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

        public override string GetText()
        {
            var result = "";

            result += _buttons.HasFlag(MouseButtons.Left)  ? "L" : ".";
            result += _buttons.HasFlag(MouseButtons.Right) ? "R" : ".";

            result += $" {_dX:D3} {_dY:D3}";

            return result;
        }

        public override bool IsEmpty()
        {
            return _buttons == MouseButtons.None
                   && _dX == 0 && _dY == 0;
        }
    }

    public class AnalogController : PsxController
    {
        private Buttons _buttons;
        private sbyte _leftX, _leftY, _rightX, _rightY;

        public AnalogController(byte[] data, bool text)
        {

            if (text)
            {
                var str = Encoding.ASCII.GetString(data);

                ushort flags = 0;

                for (var i = 0; i < 16; i++)
                {
                    flags <<= 1;

                    if (str[i] != '.')
                        flags |= 1;
                }

                _buttons = (Buttons)flags;

                _leftX  = sbyte.Parse(str.Substring(17, 3));
                _leftY  = sbyte.Parse(str.Substring(21, 3));
                _rightX = sbyte.Parse(str.Substring(25, 3));
                _rightY = sbyte.Parse(str.Substring(29, 3));
            }
            else
            {
                var x = (ushort)(data[0] | (data[1] << 8));
                _buttons = (Buttons)x;
                _leftX = (sbyte)(data[2] - 128);
                _leftY = (sbyte)(data[3] - 128);
                _rightX = (sbyte)(data[4] - 128);
                _rightY = (sbyte)(data[5] - 128);
            }
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

        public override string GetText()
        {
            var flags = "................".ToCharArray();
            var codes =  "#XO^1234LDRUSLRs".ToCharArray();

            for (var i = 0; i < 16; i++)
            {
                var flag = (Buttons)(1 << (15 - i));

                if (_buttons.HasFlag(flag))
                    flags[i] = codes[i];
            }

            var result = new string(flags);
            result += $" {_leftX:D3} {_leftY:D3} {_rightX:D3} {_rightY:D3}";

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
