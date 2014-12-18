using System.IO;
using System.Text;

namespace Yaife.Utilities
{
	public static class BinaryExtensions
	{
		/// <summary>
		/// Reads a string in ASCII.
		/// </summary>
		/// <param name="reader">The stream to read from.</param>
		/// <param name="length">The amount of bytes to read.</param>
		/// <returns>The bytes converted to an ASCII string.</returns>
		public static string ReadASCII(this BinaryReader reader, int length)
		{
			return ASCIIEncoding.ASCII.GetString(reader.ReadBytes(length));
		}

		/// <summary>
		/// Writes a string in ASCII, without padding or truncating.
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <param name="str">The string to write.</param>
		public static void WriteASCII(this BinaryWriter writer, string str)
		{
			var buffer = ASCIIEncoding.ASCII.GetBytes(str);
			writer.Write(buffer);
		}

		/// <summary>
		/// Writes a string in ASCII, padded with '\0' or truncated to fit the
		/// given length. 
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <param name="str">The string to write.</param>
		/// <param name="length">The amount of bytes that need to be written.</param>
		public static void WriteASCII(this BinaryWriter writer, string str, int length)
		{
			var realString = str.PadRight(length, '\0');
			var buffer = ASCIIEncoding.ASCII.GetBytes(realString);
			writer.Write(buffer, 0, length);
		}

		/// <summary>
		/// Reads a string in UTF8.
		/// </summary>
		/// <param name="reader">The stream to read from.</param>
		/// <param name="length">The amount of bytes to read.</param>
		/// <returns>The bytes converted to an UTF8 string.</returns>
		public static string ReadUTF8(this BinaryReader reader, int length)
		{
			return UTF8Encoding.UTF8.GetString(reader.ReadBytes(length));
		}

		/// <summary>
		/// Writes a string in ASCII, without padding or truncating.
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <param name="str">The string to write.</param>
		public static void WriteUTF8(this BinaryWriter writer, string str)
		{
			var buffer = UTF8Encoding.UTF8.GetBytes(str);
			writer.Write(buffer);
		}

		/// <summary>
		/// Writes a string in UTF8, padded with '\0' or truncated to fit the
		/// given length. 
		/// </summary>
		/// <param name="writer">The stream to write to.</param>
		/// <param name="str">The string to write.</param>
		/// <param name="length">The amount of bytes that need to be written.</param>
		public static void WriteUTF8(this BinaryWriter writer, string str, int length)
		{
			// TODO: Actually pad/truncate the buffer, as string length might
			// not equal byte length.
			var realString = str.PadRight(length, '\0');
			var buffer = UTF8Encoding.UTF8.GetBytes(realString);
			writer.Write(buffer, 0, length);
		}
	}
}
