using System.Collections.Generic;
using System.IO;

namespace GBXMapParser
{
    /// <summary>
    /// Parses ManiaPlanet maps with the GBX format.
    /// 
    /// This parser is based on the Python parser included in PyPlanet.
    /// For more information about PyPlanet, check: https://pypla.net/.
    /// </summary>
    public static class MapParser
    {
        /// <summary>
        /// Current map information object to fill.
        /// </summary>
        private static MapInformation currentMapInformation = null;

        /// <summary>
        /// Parses map information for the provided file.
        /// </summary>
        /// <param name="fileName">Name of the file to parse</param>
        /// <returns>Parsed map information</returns>
        /// <exception cref="GBXException">Thrown when the file could not be parsed properly</exception>
        public static MapInformation ReadFile(string fileName)
        {
            // Reset the current map information.
            currentMapInformation = null;

            Stream stream = File.OpenRead(fileName);

            return ReadStream(stream);
        }

        /// <summary>
        /// Parses map information for the provided byte array.
        /// </summary>
        /// <param name="bytes">Byte array to parse</param>
        /// <returns>Parsed map information</returns>
        /// <exception cref="GBXException">Thrown when the file could not be parsed properly</exception>
        public static MapInformation ReadBytes(byte[] bytes)
        {
            // Reset the current map information.
            currentMapInformation = null;

            Stream stream = new MemoryStream(bytes);

            return ReadStream(stream);
        }

        /// <summary>
        /// Parses map information for the provided stream.
        /// </summary>
        /// <param name="stream">Stream to parse</param>
        /// <returns>Parsed map information</returns>
        /// <exception cref="GBXException">Thrown when the file could not be parsed properly</exception>
        public static MapInformation ReadStream(Stream stream)
        {
            // Reset the current map information.
            currentMapInformation = null;

            // Retrieve and check class identifier to make sure the file can be read.
            stream.Seek(9, SeekOrigin.Begin);
            uint classId = StreamReader.ReadUInt(stream);

            if (classId != ((0x3 << 24) | (0x43 << 12)))
            {
                throw new GBXException("Provided file cannot be parsed by the parser.");
            }

            // Create new instance of the map information object.
            currentMapInformation = new MapInformation();

            // Parse the header into the information chunks.
            Dictionary<ChunkType, uint> chunks = ParseHeader(stream);

            // Retrieve all chunks of the header.
            ParseChunks(stream, chunks);

            // Dispose the stream.
            stream.Dispose();

            return currentMapInformation;
        }

        /// <summary>
        /// Parses the header containing the chunk information.
        /// </summary>
        /// <param name="stream">Data stream</param>
        /// <returns>Header chunk information</returns>
        private static Dictionary<ChunkType, uint> ParseHeader(Stream stream)
        {
            uint headerLength = StreamReader.ReadUInt(stream);
            uint headerChunkCount = StreamReader.ReadUInt(stream);

            Dictionary<ChunkType, uint> headerChunks = new Dictionary<ChunkType, uint>();
            for (int headerIndex = 0; headerIndex < headerChunkCount; headerIndex++)
            {
                ChunkType chunkId = (ChunkType)StreamReader.ReadUInt(stream);
                uint chunkSize = (StreamReader.ReadUInt(stream) & ~0x80000000);
                headerChunks[chunkId] = chunkSize;
            }

            return headerChunks;
        }

        /// <summary>
        /// Parse all chunks as retrieved from the header.
        /// </summary>
        /// <param name="stream">Stream to retrieve chunks from</param>
        /// <param name="chunks">Chunk information</param>
        private static void ParseChunks(Stream stream, Dictionary<ChunkType, uint> chunks)
        {
            foreach (KeyValuePair<ChunkType, uint> chunk in chunks)
            {
                // Reset the lookback version/store.
                StreamReader.ResetLookback();

                switch (chunk.Key)
                {
                    case ChunkType.MapBase:
                        ParseBaseChunk(stream);
                        break;
                    case ChunkType.MapSummary:
                        ParseSummaryChunk(stream);
                        break;
                    case ChunkType.Unknown:
                        // Just skip the unknown chunk.
                        stream.Seek(chunk.Value, SeekOrigin.Current);
                        break;
                    case ChunkType.XmlHeader:
                        currentMapInformation.HeaderXml = StreamReader.ReadString(stream);
                        break;
                    case ChunkType.Thumbnail:
                        ParseThumbnailChunk(stream, chunk.Value);
                        break;
                    case ChunkType.AuthorInfo:
                        ParseAuthorInfoChunk(stream);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the base chunk containing map type, times, editor type, etc.
        /// </summary>
        /// <param name="stream">Stream to retrieve chunk from</param>
        private static void ParseBaseChunk(Stream stream)
        {
            // Skip 5 bytes to the content.
            stream.Seek(5, SeekOrigin.Current);

            currentMapInformation.BronzeTime = StreamReader.ReadUInt(stream);
            currentMapInformation.SilverTime = StreamReader.ReadUInt(stream);
            currentMapInformation.GoldTime = StreamReader.ReadUInt(stream);
            currentMapInformation.AuthorTime = StreamReader.ReadUInt(stream);

            currentMapInformation.Price = StreamReader.ReadUInt(stream);
            currentMapInformation.IsMultilap = (StreamReader.ReadUInt(stream) == 1);
            currentMapInformation.MapTypeId = StreamReader.ReadUInt(stream);

            // Skip 4 bytes for the next bit of content.
            stream.Seek(4, SeekOrigin.Current);

            currentMapInformation.AuthorScore = StreamReader.ReadUInt(stream);
            currentMapInformation.Editor = (StreamReader.ReadUInt(stream) == 1) ? "Simple" : "Advanced";

            // Skip 4 bytes for the next bit of content.
            stream.Seek(4, SeekOrigin.Current);

            currentMapInformation.Checkpoints = StreamReader.ReadUInt(stream);
            currentMapInformation.Laps = StreamReader.ReadUInt(stream);
        }

        /// <summary>
        /// Parses the summary chunk containing UID, environment, author login, name, etc.
        /// </summary>
        /// <param name="stream">Stream to retrieve chunk from</param>
        private static void ParseSummaryChunk(Stream stream)
        {
            // Skip 1 byte to the content.
            stream.Seek(1, SeekOrigin.Current);

            currentMapInformation.UId = StreamReader.ReadLookbackString(stream);
            currentMapInformation.Environment = StreamReader.ReadLookbackString(stream);
            currentMapInformation.AuthorLogin = StreamReader.ReadLookbackString(stream);
            currentMapInformation.Name = StreamReader.ReadString(stream);

            // Skip 5 bytes for the next bit of content.
            stream.Seek(5, SeekOrigin.Current);

            // Read a normally empty string.
            StreamReader.ReadString(stream);

            currentMapInformation.Mood = StreamReader.ReadLookbackString(stream);
            currentMapInformation.DecorationEnvironmentId = StreamReader.ReadLookbackString(stream);
            currentMapInformation.DecorationEnvironmentAuthor = StreamReader.ReadLookbackString(stream);

            // Skip (4 * 4 + 16) bytes for the next bit of content.
            stream.Seek((4 * 4 + 16), SeekOrigin.Current);

            currentMapInformation.MapType = StreamReader.ReadString(stream);
            currentMapInformation.MapStyle = StreamReader.ReadString(stream);

            // Skip 9 bytes for the next bit of content.
            stream.Seek(9, SeekOrigin.Current);

            currentMapInformation.TitleId = StreamReader.ReadLookbackString(stream);
        }

        /// <summary>
        /// Parses the thumbnail chunk containing thumbnail and comments.
        /// </summary>
        /// <param name="stream">Stream to retrieve chunk from</param>
        /// <param name="chunkSize">Chunk size as provided by the header information</param>
        private static void ParseThumbnailChunk(Stream stream, uint chunkSize)
        {
            currentMapInformation.HasThumbnail = (StreamReader.ReadUInt(stream) == 1);

            if (currentMapInformation.HasThumbnail)
            {
                uint thumbnailSize = StreamReader.ReadUInt(stream);

                // Skip the XML opening thumbnail tag.
                stream.Seek(15, SeekOrigin.Current);

                // For now: don't parse the thumbnail.
                currentMapInformation.Thumbnail = StreamReader.ReadBytes(stream, thumbnailSize);

                // Skip the XML closing thumbnail and opening comments tag.
                stream.Seek((16 + 10), SeekOrigin.Current);

                currentMapInformation.Comments = StreamReader.ReadString(stream);

                // Skip the closing XML comments tag.
                stream.Seek(11, SeekOrigin.Current);
            }
            else
            {
                // Skip the chunk content.
                stream.Seek((chunkSize - 4), SeekOrigin.Current);
            }
        }

        /// <summary>
        /// Parses the author information chunk.
        /// </summary>
        /// <param name="stream">Stream to retrieve chunk from</param>
        private static void ParseAuthorInfoChunk(Stream stream)
        {
            // Skip 4 bytes to the content.
            stream.Seek(4, SeekOrigin.Current);

            currentMapInformation.AuthorVersion = StreamReader.ReadUInt(stream);

            // The login has already been read, no need to assign it again.
            string authorLogin = StreamReader.ReadString(stream);

            currentMapInformation.AuthorNickName = StreamReader.ReadString(stream);
            currentMapInformation.AuthorZone = StreamReader.ReadString(stream);
            currentMapInformation.AuthorExtra = StreamReader.ReadString(stream);
        }
    }
}
