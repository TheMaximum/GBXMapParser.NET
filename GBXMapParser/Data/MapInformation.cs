namespace GBXMapParser
{
    /// <summary>
    /// Object containing all information parsed from the map.
    /// </summary>
    public class MapInformation
    {
        #region Summary chunk

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public string UId { get; set; }

        /// <summary>
        /// Map name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Map name stripped of ManiaPlanet styling.
        /// </summary>
        public string NameStripped { get; set; }

        /// <summary>
        /// Author login.
        /// </summary>
        public string AuthorLogin { get; set; }

        /// <summary>
        /// Title identifier.
        /// </summary>
        public string TitleId { get; set; }

        /// <summary>
        /// Environment name.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Map mood.
        /// </summary>
        public string Mood { get; set; }

        /// <summary>
        /// Decoration environment identifier.
        /// </summary>
        public string DecorationEnvironmentId { get; set; }

        /// <summary>
        /// Decoration environment author.
        /// </summary>
        public string DecorationEnvironmentAuthor { get; set; }

        /// <summary>
        /// Map type.
        /// </summary>
        public string MapType { get; set; }

        /// <summary>
        /// Map style.
        /// </summary>
        public string MapStyle { get; set; }

        #endregion Summary chunk

        #region Base chunk

        /// <summary>
        /// Map type identifier.
        /// </summary>
        public uint MapTypeId { get; set; }

        /// <summary>
        /// Whether the map is a multilap map.
        /// </summary>
        public bool IsMultilap { get; set; }

        /// <summary>
        /// Amount of laps on the map.
        /// </summary>
        public uint Laps { get; set; }

        /// <summary>
        /// Amount of checkpoints in the map.
        /// </summary>
        public uint Checkpoints { get; set; }

        /// <summary>
        /// Price of the map, i.e. how heavy it is on the computer.
        /// </summary>
        public uint Price { get; set; }

        /// <summary>
        /// Editor type used to create the map.
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// Time driven by the author when validating the map.
        /// </summary>
        public uint AuthorTime { get; set; }

        /// <summary>
        /// Score achieved by the author when validating the map.
        /// </summary>
        public uint AuthorScore { get; set; }

        /// <summary>
        /// Gold time, as set by the author.
        /// </summary>
        public uint GoldTime { get; set; }

        /// <summary>
        /// Silver time, as set by the author.
        /// </summary>
        public uint SilverTime { get; set; }

        /// <summary>
        /// Bronze time, as set by the author.
        /// </summary>
        public uint BronzeTime { get; set; }

        #endregion Base chunk

        #region Author information chunk

        /// <summary>
        /// Editor version of the author.
        /// </summary>
        public uint AuthorVersion { get; set; }

        /// <summary>
        /// Nickname of the author at time of map creation.
        /// </summary>
        public string AuthorNickName { get; set; }

        /// <summary>
        /// Nickname of the author stripped of ManiaPlanet styling.
        /// </summary>
        public string AuthorNickNameStripped { get; set; }

        /// <summary>
        /// Zone of the author at time of map creation.
        /// </summary>
        public string AuthorZone { get; set; }

        /// <summary>
        /// Extra information about author.
        /// </summary>
        public string AuthorExtra { get; set; }

        #endregion Author information chunk

        #region Header XML chunk

        /// <summary>
        /// XML variant of the header (does not contain all the data).
        /// </summary>
        public string HeaderXml { get; set; }

        #endregion Header XML chunk

        #region Thumbnail chunk

        /// <summary>
        /// Whether the map has a thumbnail.
        /// </summary>
        public bool HasThumbnail { get; set; }

        /// <summary>
        /// JPEG thumbnail image.
        /// </summary>
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// Map comments.
        /// </summary>
        public string Comments { get; set; }

        #endregion Thumbnail chunk
    }
}
