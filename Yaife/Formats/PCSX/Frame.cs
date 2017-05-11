using System;

namespace Yaife.Formats.PCSX
{
    [Flags]
    public enum Console : byte
    {
        None = 0x00,
        Reset = 0x01,
        ToggleCd = 0x02,
        SioHack = 0x04,
        SpuHack = 0x08,
        Cheats = 0x10,
        ResidentEvilHack = 0x20,
        ParasiteEveHack = 0x40
    }

    public class Frame : IFrame
    {
        private Console _console;
        private readonly PsxController _controller1;
        private readonly PsxController _controller2;
        private readonly PsxControllerType _type1;
        private readonly PsxControllerType _type2;

        public Frame()
            : this(new byte[2], PsxControllerType.StandardController, new byte[2], PsxControllerType.StandardController, 0)
        { }

        public Frame(byte[] data1, PsxControllerType port1, byte[] data2, PsxControllerType port2, byte control)
        {
            _type1 = port1;
            _type2 = port2;
            _controller1 = PsxController.GetController(port1, data1);
            _controller2 = PsxController.GetController(port2, data2);
            _console = (Console)control;
        }

        public object[] ToStrings()
        {
            var p1 = _controller1.ToStrings();
            var p2 = _controller2.ToStrings();

            var result = new object[p1.Length + p2.Length + 1];
            Array.Copy(p1, 0, result, 0,         p1.Length);
            Array.Copy(p2, 0, result, p1.Length, p2.Length);
            result[result.Length - 1] = _console.ToString();

            return result;
        }

        public void Parse(string[] strings)
        {
            var l1 = PsxController.GetHeaders(_type1).Length;
            var l2 = PsxController.GetHeaders(_type2).Length;

            var s1 = new string[l1];
            Array.Copy(strings, 0, s1, 0, l1);
            var s2 = new string[l2];
            Array.Copy(strings, l1, s2, 0, l2);

            _controller1.Parse(s1);
            _controller2.Parse(s2);
            _console = (Console)Enum.Parse(typeof(Console), strings[strings.Length - 1]);
        }

        public byte[] GetBytes()
        {
            var b1 = _controller1.GetBytes();
            var b2 = _controller2.GetBytes();
            var result = new byte[b1.Length + b2.Length + 1];

            Array.Copy(b1, 0, result, 0,         b1.Length);
            Array.Copy(b2, 0, result, b1.Length, b2.Length);
            result[result.Length - 1] = (byte)_console;

            return result;
        }

        public bool IsEmpty()
        {
            return _controller1.IsEmpty()
                   && _controller2.IsEmpty()
                   && _console == Console.None;
        }
    }
}
