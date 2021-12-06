/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

namespace DataEvaluatorX.lizzie.exceptions
{
    /// <summary>
    /// Exception thrown if Lizzie for some reasons could not bind to a method.
    /// </summary>
    public class LizzieBindingException : LizzieException
    {
        /// <summary>
        /// Creates a new exception with the specified message.
        /// </summary>
        /// <param name="message">Message containing more information about the exception.</param>
        public LizzieBindingException(string message)
            : base(message)
        { }
    }
}
