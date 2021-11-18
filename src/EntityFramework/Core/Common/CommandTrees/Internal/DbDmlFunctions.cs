// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class DbDmlQueryFunctions
    {
        private static readonly MethodInfo _miBatchInsertDynamic = typeof(DbDmlQueryFunctions).GetMethod(nameof(BatchInsertDynamic), BindingFlags.Public | BindingFlags.Static);
        
        public static IQueryable<int> BatchDelete<TSource>(IQueryable<TSource> source, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchDelete(e, false, -1), source, withRowCount, limit);
        }

        public static IQueryable<int> BatchDeleteJoin<TQuery, TEntity>(IQueryable<TQuery> source, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchDeleteJoin<TQuery, TEntity>(e, false, -1), source, withRowCount, limit);
        }

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
        
        internal abstract class DynObjectQueryWrapper
        {
            public ObjectQuery ObjectQuery { get; protected set; }
        }
        
        internal sealed class DynObjectQueryWrapper<TSource> : DynObjectQueryWrapper, IQueryable<TSource>
        {
            public DynObjectQueryWrapper(ObjectQuery objectQuery)
            {
                ObjectQuery = objectQuery;
            }

            public IEnumerator<TSource> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Type ElementType => typeof(TSource);
            public Expression Expression => throw new NotImplementedException();
            public IQueryProvider Provider => throw new NotImplementedException();
        }
        
        public static IQueryable<int> BatchInsertDynamic<TSource>(IQueryable<TSource> source, string tempTableName, DynamicEntitySetOptions tempTableOptions, bool withRowCount)
        {
            if (source is ObjectQuery<TSource> objectQuery && string.IsNullOrEmpty(tempTableOptions.UniqueSetName))
            {
                var wrapper = new DynObjectQueryWrapper<TSource>(objectQuery);

                return source.Provider.CreateQuery<int>(
                    Expression.Call(
                        _miBatchInsertDynamic.MakeGenericMethod(typeof(TSource)),
                        Expression.Constant(wrapper, typeof(DynObjectQueryWrapper<TSource>)),
                        Expression.Constant(tempTableName, typeof(string)),
                        Expression.Constant(tempTableOptions, typeof(DynamicEntitySetOptions)),
                        Expression.Constant(withRowCount, typeof(bool))
                    ));
            }
            
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchInsertDynamic(e, null, null, false), source, tempTableName, tempTableOptions, withRowCount);
        }
        
        public static IQueryable<int> BatchUpdateDynamic<TSource>(IQueryable<TSource> source, DynamicEntitySetOptions tempTableOptions, Expression<Func<TSource, TSource>> selector, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchUpdateDynamic(e, null, null, false, -1), source, tempTableOptions, selector, withRowCount, limit);
        }
        
        public static IQueryable<int> BatchUpdateJoinDynamic<TQuery, TEntity>(IQueryable<TQuery> source, DynamicEntitySetOptions tempTableOptions, Expression<Func<TQuery, TEntity>> selector, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchUpdateJoinDynamic<TQuery, TEntity>(e, null, null, false, -1), source, tempTableOptions, selector, withRowCount, limit);
        }
        
        public static IQueryable<int> BatchDeleteDynamic<TSource>(IQueryable<TSource> source, DynamicEntitySetOptions tempTableOptions, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchDeleteDynamic(e, null, false, -1), source, tempTableOptions, withRowCount, limit);
        }

        public static IQueryable<int> BatchDeleteJoinDynamic<TQuery, TEntity>(IQueryable<TQuery> source, DynamicEntitySetOptions tempTableOptions, bool withRowCount, int limit)
        {
            return EntityEnumerableExtensions.BootstrapHelper.BootstrapFunction(e => BatchDeleteJoinDynamic<TQuery, TEntity>(e, null, false, -1), source, tempTableOptions, withRowCount, limit);
        }

    }
}
