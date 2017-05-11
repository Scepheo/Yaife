using System;
using System.Globalization;
using System.Text;

namespace Yaife.Utilities
{
    public static class HexString
    {
        /// <summary>
        /// Parses the hexadecimal representation of a byte array. Assumes
        /// hyphens ("-") as separators for the bytes.
        /// </summary>
        public static byte[] Parse(string str)
        {
            var split = str.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            var bytes = new byte[split.Length];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(split[i], NumberStyles.HexNumber);
            }

            return bytes;
        }

        /// <summary>
        /// Turns a byte array into a string of its hexadecimal representation.
        /// Bytes will be separated by a hyphen ("-").
        /// </summary>
        /// <param name="array">The array to convert.</param>
        /// <param name="offset">The index of the first byte to convert.</param>
        /// <returns>The hexadecimal representation of a byte array.</returns>
        public static string ToHexString(this byte[] array, int offset = 0)
        {
            return array.ToHexString(offset, array.Length - offset);
        }

        /// <summary>
        /// Turns a byte array into a string of its hexadecimal representation.
        /// Bytes will be separated by a hyphen ("-").
        /// </summary>
        /// <param name="array">The array to convert.</param>
        /// <param name="offset">The index of the first byte to convert.</param>
        /// <param name="length">The amount of bytes to convert.</param>
        /// <returns>The hexadecimal representation of a byte array.</returns>
        public static string ToHexString(this byte[] array, int offset, int length)
        {
            return BitConverter.ToString(array, offset, length);
        }
    }
}
