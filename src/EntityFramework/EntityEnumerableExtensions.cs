// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity
{
    using System.Collections.Generic;
    using System.Data.Entity.Resources;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Provide extension for IEnumerable
    /// </summary>
    public static class EntityEnumerableExtensions
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence
        /// </summary>
        /// <param name="source">The sequence to return elements from</param>
        /// <param name="count">The number of elements to return</param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns>An IEnumerable&lt;T&gt; that contains the specified number of elements from the start of the input sequence.</returns>
        /// <remarks>
        /// Used when you want to return two or more rows that tie for last place in the limited results set. Must be used with the ORDER BY clause. WITH TIES may cause more rows to be returned than the value specified in expression. For example, if expression is set to 5 but 2 additional rows match the values of the ORDER BY columns in row 5, the result set will contain 7 rows. 
        /// </remarks>
        public static IQueryable<TSource> TakeWithTies<TSource>(this IQueryable<TSource> source, int count)
        {
            return BootstrapHelper.BootstrapFunction(e => e.TakeWithTies(0), source, count);
        }
        
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence
        /// </summary>
        /// <param name="source">The sequence to return elements from</param>
        /// <param name="count">The number of elements to return</param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns>An IEnumerable&lt;T&gt; that contains the specified number of elements from the start of the input sequence.</returns>
        /// <remarks>
        /// Used when you want to return two or more rows that tie for last place in the limited results set. Must be used with the ORDER BY clause. WITH TIES may cause more rows to be returned than the value specified in expression. For example, if expression is set to 5 but 2 additional rows match the values of the ORDER BY columns in row 5, the result set will contain 7 rows. 
        /// </remarks>
        public static IEnumerable<TSource> TakeWithTies<TSource>(this IEnumerable<TSource> source, int count)
        {
            return BootstrapHelper.BootstrapFunction(e => e.TakeWithTies(0), source, count);
        }
        
        internal static class BootstrapHelper
        {
            public static IQueryable<TOut> BootstrapFunction<TIn, TOut, TParam0>(Expression<Func<IQueryable<TIn>, IQueryable<TOut>>> methodExpression, IQueryable<TIn> collection, TParam0 p0)
            {
                // We could use methodExpression directly here, but it seems marginally better (and consistent with
                // previous versions) to use a constant expression for the parameter.
                return collection.Provider.CreateQuery<TOut>(
                    Expression.Call(((MethodCallExpression)methodExpression.Body).Method, collection.Expression, Expression.Constant(p0, typeof(TParam0))));
            }
        
            public static IQueryable<TOut> BootstrapFunction<TIn, TOut, TParam0, TParam1>(
                Expression<Func<IQueryable<TIn>, IQueryable<TOut>>> methodExpression, IQueryable<TIn> collection, 
                TParam0 p0, TParam1 p1)
            {
                // We could use methodExpression directly here, but it seems marginally better (and consistent with
                // previous versions) to use a constant expression for the parameter.
                return collection.Provider.CreateQuery<TOut>(
                    Expression.Call(
                        ((MethodCallExpression)methodExpression.Body).Method, collection.Expression,
                        Expression.Constant(p0, typeof(TParam0)),
                        Expression.Constant(p1, typeof(TParam1))
                    ));
            }
            
            public static IQueryable<TOut> BootstrapFunction<TIn, TOut, TParam0, TParam1, TParam2>(
                Expression<Func<IQueryable<TIn>, IQueryable<TOut>>> methodExpression, IQueryable<TIn> collection, 
                TParam0 p0, TParam1 p1, TParam2 p2)
            {
                // We could use methodExpression directly here, but it seems marginally better (and consistent with
                // previous versions) to use a constant expression for the parameter.
                return collection.Provider.CreateQuery<TOut>(
                    Expression.Call(
                        ((MethodCallExpression)methodExpression.Body).Method, collection.Expression,
                        Expression.Constant(p0, typeof(TParam0)),
                        Expression.Constant(p1, typeof(TParam1)),
                        Expression.Constant(p2, typeof(TParam2))
                    ));
            }
            
            public static IQueryable<TOut> BootstrapFunction<TIn, TOut, TParam0, TParam1, TParam2, TParam3>(
                Expression<Func<IQueryable<TIn>, IQueryable<TOut>>> methodExpression, IQueryable<TIn> collection, 
                TParam0 p0, TParam1 p1, TParam2 p2, TParam3 p3)
            {
                // We could use methodExpression directly here, but it seems marginally better (and consistent with
                // previous versions) to use a constant expression for the parameter.
                return collection.Provider.CreateQuery<TOut>(
                    Expression.Call(
                        ((MethodCallExpression)methodExpression.Body).Method, collection.Expression,
                        Expression.Constant(p0, typeof(TParam0)),
                        Expression.Constant(p1, typeof(TParam1)),
                        Expression.Constant(p2, typeof(TParam2)),
                        Expression.Constant(p3, typeof(TParam3))
                    ));
            }
            
            public static IEnumerable<TOut> BootstrapFunction<TIn, TOut, TParam>(Expression<Func<IQueryable<TIn>, IQueryable<TOut>>> methodExpression, IEnumerable<TIn> collection, TParam p0)
            {
                if (collection is IQueryable<TIn> asQueryable)
                {
                    // We could use methodExpression directly here, but it seems marginally better (and consistent with
                    // previous versions) to use a constant expression for the parameter.
                    return asQueryable.Provider.CreateQuery<TOut>(
                        Expression.Call(((MethodCallExpression)methodExpression.Body).Method, asQueryable.Expression, Expression.Constant(p0, typeof(TParam))));
                }

                throw new NotSupportedException(Strings.ELinq_DbFunctionDirectCall);
            }
            
        }
    }
}
