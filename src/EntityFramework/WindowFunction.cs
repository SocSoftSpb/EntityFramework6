// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity
{
    /// <summary>
    /// Partitioning for window function
    /// </summary>
    // ReSharper disable once ClassCannotBeInstantiated
    public sealed class Partition
    {
        private Partition() { }

        /// <summary>
        /// First partition argument for window function
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static Partition By<T>(T arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// More partition argument(s) for window function
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Partition ThenBy<T>(T arg)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Ordering for window function
    /// </summary>
    // ReSharper disable once ClassCannotBeInstantiated
    public sealed class Order
    {
        private Order() { }

        /// <summary>
        /// First ascending order argument for window function
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static Order By<T>(T arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// First descending order argument for window function
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static Order ByDescending<T>(T arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// More ascending order argument(s) for window function
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Order ThenBy<T>(T arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// More descending order argument(s) for window function
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Order ThenByDescending<T>(T arg)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Represents various <see cref="WindowFunction{TResult}"/> factory
    /// </summary>
    public static class WindowFunction
    {
        #region Ranking

        /// <summary>
        /// Numbers the output of a result set. More specifically, returns the sequential number of a row within a partition of a result set, starting at 1 for the first row in each partition. 
        /// </summary>
        /// <remarks>
        /// ROW_NUMBER and RANK are similar. ROW_NUMBER numbers all rows sequentially (for example 1, 2, 3, 4, 5). RANK provides the same numeric value for ties (for example 1, 2, 2, 4, 5). 
        /// </remarks>
        public static WindowFunction<long> RowNumber()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the rank of each row within the partition of a result set. The rank of a row is one plus the number of ranks that come before the row in question.
        /// </summary>
        /// <remarks>
        /// ROW_NUMBER and RANK are similar. ROW_NUMBER numbers all rows sequentially (for example 1, 2, 3, 4, 5). RANK provides the same numeric value for ties (for example 1, 2, 2, 4, 5). 
        /// </remarks>
        public static WindowFunction<long> Rank()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the rank of rows within the partition of a result set, without any gaps in the ranking. The rank of a row is one plus the number of distinct ranks that come before the row in question.
        /// </summary>
        public static WindowFunction<long> DenseRank()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Distributes the rows in an ordered partition into a specified number of groups. The groups are numbered, starting at one. For each row, NTILE returns the number of the group to which the row belongs. 
        /// </summary>
        /// <param name="groupCount">A positive integer constant expression that specifies the number of groups into which each partition must be divided.</param>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "groupCount")]
        public static WindowFunction<long> NTile(long groupCount)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Aggregates

        #region COUNT

        /// <summary>
        /// SQL function Count(*). Returns the number of items in a group. This includes NULL values and duplicates. 
        /// </summary>
        public static WindowFunction<int> Count()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(bool? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(byte? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(DateTime? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(decimal? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(double? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(short? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(int? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(long? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(float? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(string arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int> Count(Guid? arg)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region COUNT_BIG

        /// <summary>
        /// SQL function Count_Big(*). Returns the number of items in a group. This includes NULL values and duplicates. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(bool? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(byte? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(DateTime? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(decimal? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(double? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(short? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(int? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(long? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(float? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(string arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Count_Big(arg). Evaluates <paramref name="arg"/> for each row in a group and returns the number of nonnull values.
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long> LongCount(Guid? arg)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region AVG

        /// <summary>
        /// SQL function Avg. Returns the average of the values in a group. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<decimal?> Avg(decimal? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Avg. Returns the average of the values in a group. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<double?> Avg(double? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Avg. Returns the average of the values in a group. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int?> Avg(int? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Avg. Returns the average of the values in a group. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long?> Avg(long? arg)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region SUM

        /// <summary>
        /// SQL function Sum. Returns the sum of all the values, in the expression. SUM can be used with numeric columns only. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<decimal?> Sum(decimal? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Sum. Returns the sum of all the values, in the expression. SUM can be used with numeric columns only. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<double?> Sum(double? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Sum. Returns the sum of all the values, in the expression. SUM can be used with numeric columns only. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int?> Sum(int? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Sum. Returns the sum of all the values, in the expression. SUM can be used with numeric columns only. Null values are ignored. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long?> Sum(long? arg)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region MAX

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<byte?> Max(byte? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<DateTime?> Max(DateTime? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<decimal?> Max(decimal? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<double?> Max(double? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<short?> Max(short? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int?> Max(int? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long?> Max(long? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<float?> Max(float? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<string> Max(string arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<byte[]> Max(byte[] arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Max(). Returns the maximum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<DateTimeOffset> Max(DateTimeOffset arg)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region MIN

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<byte?> Min(byte? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<DateTime?> Min(DateTime? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<decimal?> Min(decimal? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<double?> Min(double? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<short?> Min(short? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<int?> Min(int? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<long?> Min(long? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<float?> Min(float? arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<string> Min(string arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<byte[]> Min(byte[] arg)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// SQL function Min(). Returns the minimum value in the expression. Ignores any null values. 
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "arg")]
        public static WindowFunction<DateTimeOffset> Min(DateTimeOffset arg)
        {
            throw new NotSupportedException();
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// SQL window function
    /// </summary>
    public sealed class WindowFunction<TResult>
    {
        internal WindowFunction()
        {
        }

        /// <summary>
        /// Calculate window function over entire row set
        /// </summary>
        public TResult Over()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Calculate window function over partitioned row groups
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "partition")]
        public TResult Over(Partition partition)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Calculate window function over ordered rows
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "order")]
        public TResult Over(Order order)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Calculate window function over ordered rows in partitioned row groups
        /// </summary>
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "partition")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "order")]
        public TResult Over(Partition partition, Order order)
        {
            throw new NotSupportedException();
        }
    }

}
