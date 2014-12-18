namespace Yaife.Utilities
{
	public static class CRC32
	{
		private const uint kCrcPoly = 0xEDB88320;
		private static uint[] crcTable;

		/// <summary>
		/// Calculates the 32 bit CRC value.
		/// </summary>
		/// <param name="data">The data to calculate the CRC hash for.</param>
		/// <returns>The 32 bit CRC hash of the data.</returns>
		public static uint CalculateCrc(byte[] data)
		{
			return crcUpdate(0xFFFFFFFF, data) ^ 0xFFFFFFFF;
		}

		private static uint crcUpdate(uint v, byte[] data)
		{
			if (crcTable == null)
				initCrcTable();

			for (int i = 0; i < data.Length; i++)
				v = (crcTable[((v) ^ (data[i])) & 0xFF] ^ ((v) >> 8));

			return v;
		}

		private static void initCrcTable()
		{
			crcTable = new uint[256];

			for (uint i = 0; i < 256; i++)
			{
				uint r = i;
				
				for (int j = 0; j < 8; j++)
					r = (r >> 1) ^ (kCrcPoly & ~((r & 1) - 1));

				crcTable[i] = r;
			}
		}
	}
}
