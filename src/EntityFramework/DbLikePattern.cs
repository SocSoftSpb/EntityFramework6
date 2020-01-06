// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace System.Data.Entity 
{
    using System.Collections.Generic;
    using System.Data.Entity.Utilities;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// Pattern for LikeCommon
    /// </summary>
    public sealed class DbLikePattern
    {
        /// <summary>
        /// Pattern fragments
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<DbLikePatterFragment> Fragments { get; } = new List<DbLikePatterFragment>();

        /// <summary>
        /// Add pattern fragment
        /// </summary>
        /// <param name="type">Fragment type</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public DbLikePattern Add(DbLikePatternType type, string value = null)
        {
            Fragments.Add(new DbLikePatterFragment(type, value));
            return this;
        }

        /// <summary>
        /// Add plain text for comparison
        /// </summary>
        /// <param name="value">Text</param>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "PlainText")]
        public DbLikePattern PlainText(string value) => Add(DbLikePatternType.PlainText, value);

        /// <summary>Any substring '%'</summary>
        public DbLikePattern AnyString() => Add(DbLikePatternType.AnyString);

        /// <summary>Any character '_'</summary>
        public DbLikePattern AnyChar() => Add(DbLikePatternType.AnyChar);

        /// <summary>Characters set '[<paramref name="charset"/>]'</summary>
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        public DbLikePattern Charset(string charset) => Add(DbLikePatternType.Charset, charset);

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Fragments.Count == 0)
                return string.Empty;

            var sb = new StringBuilder(128);
            foreach (var fragment in Fragments)
            {
                switch (fragment.Type)
                {
                    case DbLikePatternType.AnyString:
                        sb.Append('%');
                        break;
                    case DbLikePatternType.AnyChar:
                        sb.Append('_');
                        break;
                    case DbLikePatternType.Charset:
                        sb.Append('[').Append(fragment.Value).Append(']');
                        break;
                    case DbLikePatternType.PlainText:
                        sb.Append(fragment.Value);
                        break;
                }
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Pattern fragment for LikeCommon
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    public struct DbLikePatterFragment
    {
        internal DbLikePatterFragment(DbLikePatternType type, string value = null)
        {
            Type = type;
            if (type == DbLikePatternType.PlainText
             || type == DbLikePatternType.Charset)
            {
                Check.NotNull(value, nameof(value));
            }
            Value = value;
        }

        /// <summary>
        /// Pattern type
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public DbLikePatternType Type { get; }
        /// <summary>
        /// String value
        /// </summary>
        public string Value { get; }
    }

    /// <summary>
    /// Pattern type for LikeCommon
    /// </summary>
    public enum DbLikePatternType
    {
        /// <summary>Plain text</summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "PlainText")]
        PlainText = 0,
        /// <summary>Any substring '%'</summary>
        AnyString = 1,
        /// <summary>Any character '_'</summary>
        AnyChar = 2,
        /// <summary>Characters set '[abc]'</summary>
        Charset = 3
    }
}
