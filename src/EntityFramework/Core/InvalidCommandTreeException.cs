// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core
{
    using System.ComponentModel;
    using System.Data.Entity.Resources;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown to indicate that a command tree is invalid.
    /// </summary>
    [Serializable]
    public sealed class InvalidCommandTreeException : DataException /*InvalidQueryException*/
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.InvalidCommandTreeException" /> class  with a default message.
        /// </summary>
        public InvalidCommandTreeException()
            : base(Strings.Cqt_Exceptions_InvalidCommandTree)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.InvalidCommandTreeException" /> class with the specified message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InvalidCommandTreeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.InvalidCommandTreeException" /> class  with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">
        /// The exception that is the cause of this <see cref="T:System.Data.Entity.Core.InvalidCommandTreeException" />.
        /// </param>
        public InvalidCommandTreeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // <summary>
        // Constructs a new InvalidCommandTreeException from the specified serialization info and streaming context.
        // </summary>
#if NET10_0_OR_GREATER
        [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        private InvalidCommandTreeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
