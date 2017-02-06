namespace GGS.CakeBox.Utils
{
    public static class ByteConverter
    {
        public static readonly int ByteInBits = 8;

        /// <summary>
        /// Converts 4 bytes into 1 int!
        /// a = first 8 bit
        /// d = last 8 bit
        /// </summary>
        /// <returns>return int from 4 bytes</returns>
        public static int BytesToInt(byte a, byte b, byte c, byte d)
        {
            return a << ByteInBits * 3 | b << ByteInBits * 2 | c << ByteInBits | d;
        }

        /// <summary>
        /// Converts 1 int to 4 bytes in big endian 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>return byte array which is representing the int value</returns>
        public static byte[] IntToBytes(this int value)
        {
            byte[] bytes = new byte[sizeof(int)];

            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = (byte)(value >> ((bytes.Length - 1 - i) * ByteInBits));
            }
            return bytes;
        }
    }
}