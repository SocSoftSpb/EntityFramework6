// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
    using System.Data.Entity.Resources;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Functions-markers for batch DML operations
    /// </summary>
    public static class DbDmlFunctions
    {
        /// <summary>
        /// Marker of Delete operation. For internal use only
        /// </summary>
        public static int DeleteMarker<TEntity>(TEntity entity)
        {
            throw new NotSupportedException(Strings.ELinq_DbFunctionDirectCall);
        }

        /// <summary>
        /// Marker of Delete operation with returns RowsCount. For internal use only
        /// </summary>
        public static int DeleteMarkerRowCount<TEntity>(TEntity entity)
        {
            throw new NotSupportedException(Strings.ELinq_DbFunctionDirectCall);
        }
    }

    public static class DbDmlQueryFunctions
    {
        public static IQueryable<int> BatchUpdate<TSource>(IQueryable<TSource> source, Expression<Func<TSource, TSource>> selector, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchUpdate(e, null, false, -1), source, selector, withRowCount, limit);
        }

        public static IQueryable<int> BatchUpdateJoin<TQuery, TEntity>(IQueryable<TQuery> source, Expression<Func<TQuery, TEntity>> selector, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchUpdateJoin<TQuery, TEntity>(e, null, false, -1), source, selector, withRowCount, limit);
        }

        public static IQueryable<int> BatchInsert<TSource>(IQueryable<TSource> source, bool withRowCount)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchInsert(e, false), source, withRowCount);
        }
    }
}
