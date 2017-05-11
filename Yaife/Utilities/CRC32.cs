namespace Yaife.Utilities
{
    public static class Crc32
    {
        private const uint KCrcPoly = 0xEDB88320;
        private static readonly uint[] CrcTable = InitCrcTable();

        /// <summary>
        /// Calculates the 32 bit CRC value.
        /// </summary>
        /// <param name="bytes">The data to calculate the CRC hash for.</param>
        /// <returns>The 32 bit CRC hash of the data.</returns>
        public static uint CalculateCrc(byte[] bytes)
        {
            var value = 0xFFFFFFFF;

            foreach (var b in bytes)
            {
                value = CrcTable[(value ^ b) & 0xFF] ^ (value >> 8);
            }

            return value ^ 0xFFFFFFFF;
        }

        private static uint[] InitCrcTable()
        {
            var crcTable = new uint[256];

            for (uint i = 0; i < 256; i++)
            {
                var r = i;

                for (var j = 0; j < 8; j++)
                {
                    r = (r >> 1) ^ (KCrcPoly & ~((r & 1) - 1));
                }

                crcTable[i] = r;
            }

            return crcTable;
        }
    }
}
