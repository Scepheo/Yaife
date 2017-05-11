using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Yaife.Formats.Hourglass
{
    public class Frame : IFrame
    {
        private readonly List<Keys> _keys;

        public object[] ToStrings()
        {
            return new object[] { string.Join(", ", _keys) };
        }

        public void Parse(string[] str)
        {
            _keys.Clear();
            var split = str[0].Split(new[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var s in split)
            {
                var keyString = s.Trim();
                _keys.Add((Keys)Enum.Parse(typeof(Keys), keyString));
            }
        }

        public Frame(byte[] buffer)
        {
            _keys = new List<Keys>();

            foreach (var b in buffer)
            {
                if (b > 0)
                {
                    _keys.Add((Keys)b);
                }
            }
        }

        public Frame() : this(new byte[0])
        {
        }

        public byte[] GetBytes()
        {
            var buffer = new byte[8];

            for (var i = 0; i < 8 && i < _keys.Count; i++)
                buffer[i] = (byte)_keys[i];

            return buffer;
        }
    }
}
