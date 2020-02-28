// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace System.Data.Entity.Core.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Markers for dynamic SQL
    /// </summary>
    public static class DynamicSqlUtilities
    {
        /// <summary>
        /// Function-marker for create temporary table
        /// </summary>
        /// <typeparam name="T">Type of item in query</typeparam>
        /// <param name="query">Query representation</param>
        /// <param name="entitySetOptions">Options for this query</param>
        /// <returns>Dynamic query with given params</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "entitySetOptions")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "query")]
        public static IQueryable<T> DynamicSql<T>(string query, DynamicEntitySetOptions entitySetOptions) where T : class
        {
            throw new NotSupportedException("Direct call of Dynamic SQL is not supported");
        }
    }

    /// <summary>
    /// Dynamic EntitySet options
    /// </summary>
    public class DynamicEntitySetOptions
    {
        private readonly IList<ColumnOption> _columns = new List<ColumnOption>();

        /// <summary>
        /// Columns options collection
        /// </summary>
        public IList<ColumnOption> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// EntitySet unique name for Query Caching purposes
        /// </summary>
        public string UniqueSetName { get; set; }

        /// <summary>
        /// Default hints for query (Dynamic Table EntitySet only!)
        /// </summary>
        public TableHints DefaultTableHints { get; set; }

        /// <summary>
        /// Primary key column names
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] KeyMemberNames { get; set; }
    }

    /// <summary>
    /// Column option for Dynamic EntitySet
    /// </summary>
    public class ColumnOption
    {
        /// <summary>
        /// ColumnOption ctor
        /// </summary>
        /// <param name="propertyInfo">Property for this column</param>
        public ColumnOption(PropertyInfo propertyInfo)
        {
            Property = propertyInfo;

            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsValueType)
            {
                IsNullable = Nullable.GetUnderlyingType(propertyType) != null;
            }
            else
            {
                IsNullable = true;
            }
        }

        /// <summary>
        /// CLR Property for this column
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Column allows null values
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Name of table column
        /// </summary>
        public string ColumnName
        {
            get { return Property.Name; }
        }
    }

}