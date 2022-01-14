using System;

namespace kiki
{
    /// <summary>
    /// Base exception class for all exceptions thrown by Kiki.
    ///</summary>
    public class KikiException : Exception
    {
        /// <summary>
        /// Creates a new exception with the specified message.
        /// </summary>
        /// <param name="message">Message containing more information about the exception.</param>
        public KikiException(string message)
            : base(message)
        { }
    }
}