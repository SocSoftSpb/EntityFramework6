// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity
{
    /// <summary>
    /// IsNull / Coalesce functions
    /// </summary>
    public static class DbIsNullFunctions
    {
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static bool? DbIsNull(this bool? value, bool? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static bool DbIsNull(this bool? value, bool defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static byte? DbIsNull(this byte? value, byte? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static byte DbIsNull(this byte? value, byte defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static short? DbIsNull(this short? value, short? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static short DbIsNull(this short? value, short defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static int? DbIsNull(this int? value, int? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static int DbIsNull(this int? value, int defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static long? DbIsNull(this long? value, long? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static long DbIsNull(this long? value, long defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static decimal? DbIsNull(this decimal? value, decimal? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static decimal DbIsNull(this decimal? value, decimal defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static double? DbIsNull(this double? value, double? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static double DbIsNull(this double? value, double defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static float? DbIsNull(this float? value, float? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static float DbIsNull(this float? value, float defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static DateTime? DbIsNull(this DateTime? value, DateTime? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static DateTime DbIsNull(this DateTime? value, DateTime defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static DateTimeOffset? DbIsNull(this DateTimeOffset? value, DateTimeOffset? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static DateTimeOffset DbIsNull(this DateTimeOffset? value, DateTimeOffset defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static Guid? DbIsNull(this Guid? value, Guid? defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static Guid DbIsNull(this Guid? value, Guid defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static string DbIsNull(this string value, string defaultValue)
        {
            return value ?? defaultValue;
        }
        
        /// <summary>
        /// Null-coalesce function (ISNULL / COALESCE)
        /// </summary>
        [DbFunction("Edm", "IsNull")]
        public static byte[] DbIsNull(this byte[] value, byte[] defaultValue)
        {
            return value ?? defaultValue;
        }
        
    }
}
