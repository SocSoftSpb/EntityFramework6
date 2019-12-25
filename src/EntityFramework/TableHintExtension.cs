// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Provide extension for SQL Table hints
    /// </summary>
    public static class TableHintExtension
    {
        private static readonly MethodInfo _withTableHint;
        private static readonly MethodInfo _withTypedTableHint;
        private static readonly MethodInfo _withDefaultTableHint;
        private static readonly MethodInfo _withScopedTableHint;
        private static readonly MethodInfo _withQueryOptions;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static TableHintExtension()
        {
            _withTableHint = typeof(TableHintExtension).GetMethod(nameof(WithTableHint));
            _withDefaultTableHint = typeof(TableHintExtension).GetMethod(nameof(WithDefaultTableHint));
            _withTypedTableHint = typeof(TableHintExtension).GetMethod(nameof(WithTypeTableHint));
            _withScopedTableHint = typeof(TableHintExtension).GetMethod(nameof(WithScopedTableHint));
            _withQueryOptions = typeof(TableHintExtension).GetMethod(nameof(WithQueryOptions));
        }

        /// <summary>
        /// Add default table-level hint to all scan table operations in query.
        /// </summary>
        /// <typeparam name="TSource">Type of queryable</typeparam>
        /// <param name="source">Source query</param>
        /// <param name="hint">Query hints</param>
        /// <returns>Returns SQL Query with hints</returns>
        /// <remarks><typeparamref name="TSource"/> must be a one of mapped entity type.</remarks>
        public static IQueryable<TSource> WithDefaultTableHint<TSource>(this IQueryable<TSource> source, TableHints hint)
        {
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    _withDefaultTableHint.MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(hint, typeof(TableHints)))
            );
        }

        /// <summary>
        /// Add table-level hint to scan table operation. Query must be a one of mapped entity type.
        /// </summary>
        /// <typeparam name="TSource">Type of queryable</typeparam>
        /// <param name="source">Source query</param>
        /// <param name="hint">Query hints</param>
        /// <returns>Returns SQL Query with hints</returns>
        /// <remarks><typeparamref name="TSource"/> must be a one of mapped entity type.</remarks>
        public static IQueryable<TSource> WithTableHint<TSource>(this IQueryable<TSource> source, TableHints hint)
        {
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    _withTableHint.MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(hint, typeof(TableHints)))
            );
        }

        /// <summary>
        /// Add table-level hint for query and given table type.
        /// </summary>
        /// <typeparam name="TSource">Type of queryable</typeparam>
        /// <param name="source">Source query</param>
        /// <param name="hint">Query hints</param>
        /// <param name="entityType">Entity type for hint is applied. Null - hint is applied to entire query</param>
        /// <returns>Returns SQL Query with hints</returns>
        public static IQueryable<TSource> WithTypeTableHint<TSource>(this IQueryable<TSource> source, TableHints hint, Type entityType)
        {
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    _withTypedTableHint.MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(hint, typeof(TableHints)),
                    Expression.Constant(entityType, typeof(Type)))
            );
        }

        /// <summary>
        /// Add table-level hint to scan table operation. Hint scoped to nested scan operations.
        /// </summary>
        /// <typeparam name="TSource">Type of queryable</typeparam>
        /// <param name="source">Source query</param>
        /// <param name="hint">Query hints</param>
        /// <returns>Returns SQL Query with hints</returns>
        /// <remarks><typeparamref name="TSource"/> must be a one of mapped entity type.</remarks>
        public static IQueryable<TSource> WithScopedTableHint<TSource>(this IQueryable<TSource> source, TableHints hint)
        {
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    _withScopedTableHint.MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(hint, typeof(TableHints)))
            );
        }

        /// <summary>
        /// Add SQL query options.
        /// </summary>
        /// <typeparam name="TSource">Type of queryable</typeparam>
        /// <param name="source">Source query</param>
        /// <param name="options">Query options</param>
        /// <returns>Returns SQL Query with options</returns>
        public static IQueryable<TSource> WithQueryOptions<TSource>(this IQueryable<TSource> source, QueryOptions options)
        {
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    _withQueryOptions.MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    Expression.Constant(options, typeof(QueryOptions)))
            );
        }

    }

    /// <summary>
    /// Represents Table - Level Hints
    /// </summary>
    [Flags]
    public enum TableHints
    {
        /// <summary>
        /// No hints
        /// </summary>
        None = 0,
        /// <summary>
        /// NOLOCK
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nolock")]
        Nolock = 1,

        /// <summary>
        /// UPDLOCK
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Updlock")]
        Updlock = 2,

        /// <summary>
        /// ROWLOCK
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rowlock")]
        Rowlock = 4,

        /// <summary>
        /// HOLDLOCK
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Holdlock")]
        Holdlock = 8,

    }

    /// <summary>
    /// Represents query options (select ... OPTION (...))
    /// </summary>
    public class QueryOptions
    {
        /// <summary>
        /// Instructs the SQL Server Database Engine to generate a new, temporary plan for the query and immediately discard that plan after the query completes execution.
        /// </summary>
        /// <remarks>
        /// When compiling query plans, the RECOMPILE query hint uses the current values of any local variables in the query.
        /// </remarks>
        public bool Recompile { get; set; }

        /// <summary>
        /// Overrides the max degree of parallelism configuration option of sp_configure. 
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dop")]
        public int? MaxDop { get; set; }

        private static void AddCacheKeyOption(StringBuilder stringBuilder, ref int optionsCount, string name, string value = null)
        {
            if (optionsCount == 0)
            {
                if (stringBuilder.Length != 0)
                    stringBuilder.Append("|");
                stringBuilder.Append("opts:");
            }
            else
            {
                stringBuilder.Append(",");
            }

            stringBuilder.Append(name);
            if (value != null)
            {
                stringBuilder.Append("=").Append(value);
            }

            optionsCount++;

        }

        internal void AddCacheKey(StringBuilder stringBuilder)
        {
            var optionsCount = 0;
            if (Recompile)
                AddCacheKeyOption(stringBuilder, ref optionsCount, nameof(Recompile));

            if (MaxDop != null)
                AddCacheKeyOption(stringBuilder, ref optionsCount, nameof(MaxDop), MaxDop.Value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Check if option is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return !(Recompile || MaxDop != null);
        }
    }
}
