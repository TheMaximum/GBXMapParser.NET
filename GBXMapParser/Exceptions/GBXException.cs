using System;

namespace GBXMapParser
{
    /// <summary>
    /// Exception in the GBX parser.
    /// </summary>
    public class GBXException : Exception
    {
        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message">Exceptin message</param>
        public GBXException(string message)
            : base(message)
        {

        }
    }
}
