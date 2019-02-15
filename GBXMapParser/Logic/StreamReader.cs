using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBXMapParser
{
    /// <summary>
    /// Reads the requested data from the provided stream.
    /// </summary>
    internal static class StreamReader
    {
        /// <summary>
        /// Lookback string version.
        /// </summary>
        private static uint? lookbackVersion = null;

        /// <summary>
        /// Lookback string store.
        /// </summary>
        private static List<string> lookbackStore;

        /// <summary>
        /// Predefined messages for the lookback strings.
        /// </summary>
        private static Dictionary<uint, string> predefinedLookbackStrings = new Dictionary<uint, string>()
        {
            { 11, "Valley" },
            { 12, "Canyon" },
            { 13, "Lagoon" },
            { 17, "TMCommon" },
            { 202, "Storm" },
            { 299, "SMCommon" },
            { 10003, "Common" }
        };

        /// <summary>
        /// Reset the lookback version and store.
        /// </summary>
        internal static void ResetLookback()
        {
            lookbackVersion = null;
            lookbackStore = new List<string>();
        }

        /// <summary>
        /// Reads a 'lookback-string', which is used to compact the file to refer to the same string multiple times.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <returns>Read string</returns>
        internal static string ReadLookbackString(Stream stream)
        {
            if (!lookbackVersion.HasValue)
            {
                lookbackVersion = ReadUInt(stream);
            }

            // Read the string index.
            uint stringIndex = ReadUInt(stream);
            if (stringIndex == 0)
                return string.Empty;

            // Check if this is the first occurance of the index.
            if (((stringIndex & 0xc0000000) != 0) && ((stringIndex & 0x3fffffff) == 0))
            {
                string value = ReadString(stream);
                lookbackStore.Add(value);
                return value;
            }

            // Check if the index indicates an empty string.
            if (stringIndex == 0xffffffff)
                return string.Empty;

            // Check if it's a predefined value.
            if ((stringIndex & 0x3fffffff) == stringIndex)
                return predefinedLookbackStrings[stringIndex];

            stringIndex &= 0x3fffffff;
            uint storeIndex = (stringIndex - 1);

            if (storeIndex >= lookbackStore.Count)
                throw new GBXException(string.Format("String with offset '{0}' not found in the lookback list!", storeIndex));

            return lookbackStore[(int)storeIndex];
        }

        /// <summary>
        /// Reads a string from the stream. It contains a length (4 bytes) and the content.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <returns>Read string</returns>
        internal static string ReadString(Stream stream)
        {
            uint stringLength = ReadUInt(stream);

            byte[] rawBytes = new byte[stringLength];
            stream.Read(rawBytes, 0, (int)stringLength);

            return Encoding.UTF8.GetString(rawBytes);
        }

        /// <summary>
        /// Read an uint from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <returns>Read uint</returns>
        internal static uint ReadUInt(Stream stream)
        {
            byte[] rawBytes = ReadBytes(stream, 4);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(rawBytes);
            }

            return BitConverter.ToUInt32(rawBytes, 0);
        }

        /// <summary>
        /// Reads raw bytes from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="length">Amount of bytes to read</param>
        /// <returns>Read bytes</returns>
        internal static byte[] ReadBytes(Stream stream, uint length)
        {
            byte[] rawBytes = new byte[length];
            stream.Read(rawBytes, 0, (int)length);

            return rawBytes;
        }
    }
}
