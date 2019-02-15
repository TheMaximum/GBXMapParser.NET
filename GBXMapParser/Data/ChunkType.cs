namespace GBXMapParser
{
    /// <summary>
    /// Enum containing all the chunk types.
    /// </summary>
    internal enum ChunkType
    {
        /// <summary>
        /// Chunk identifier for the base information.
        /// </summary>
        MapBase = 0x03043002,

        /// <summary>
        /// Chunk identifier for the map summary.
        /// </summary>
        MapSummary = 0x03043003,

        /// <summary>
        /// Unknown chunk.
        /// </summary>
        Unknown = 0x03043004,

        /// <summary>
        /// Chunk identifier for the XML header.
        /// </summary>
        XmlHeader = 0x03043005,

        /// <summary>
        /// Chunk identifier for the thumbnail and comments.
        /// </summary>
        Thumbnail = 0x03043007,

        /// <summary>
        /// Chunk identifier for the author information.
        /// </summary>
        AuthorInfo = 0x03043008
    }
}
