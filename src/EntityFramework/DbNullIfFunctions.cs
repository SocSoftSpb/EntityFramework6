// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace System.Data.Entity 
{
    /// <summary>
    /// Null if default value
    /// </summary>
    public static class DbNullIfFunctions
    {
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static bool? DbNullIf(this bool? value, bool defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static bool? DbNullIf(this bool value, bool defaultValue)
        {
            return value == defaultValue ? (bool?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static byte? DbNullIf(this byte? value, byte defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static byte? DbNullIf(this byte value, byte defaultValue)
        {
            return value == defaultValue ? (byte?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static short? DbNullIf(this short? value, short defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static short? DbNullIf(this short value, short defaultValue)
        {
            return value == defaultValue ? (short?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static int? DbNullIf(this int? value, int defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static int? DbNullIf(this int value, int defaultValue)
        {
            return value == defaultValue ? (int?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static long? DbNullIf(this long? value, long defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static long? DbNullIf(this long value, long defaultValue)
        {
            return value == defaultValue ? (long?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static decimal? DbNullIf(this decimal? value, decimal defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static decimal? DbNullIf(this decimal value, decimal defaultValue)
        {
            return value == defaultValue ? (decimal?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static double? DbNullIf(this double? value, double defaultValue)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static double? DbNullIf(this double value, double defaultValue)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return value == defaultValue ? (double?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static float? DbNullIf(this float? value, float defaultValue)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static float? DbNullIf(this float value, float defaultValue)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return value == defaultValue ? (float?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static DateTime? DbNullIf(this DateTime? value, DateTime defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static DateTime? DbNullIf(this DateTime value, DateTime defaultValue)
        {
            return value == defaultValue ? (DateTime?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static DateTimeOffset? DbNullIf(this DateTimeOffset? value, DateTimeOffset defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static DateTimeOffset? DbNullIf(this DateTimeOffset value, DateTimeOffset defaultValue)
        {
            return value == defaultValue ? (DateTimeOffset?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static Guid? DbNullIf(this Guid? value, Guid defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static Guid? DbNullIf(this Guid value, Guid defaultValue)
        {
            return value == defaultValue ? (Guid?)null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static string DbNullIf(this string value, string defaultValue)
        {
            return value == defaultValue ? null : value;
        }
        
        /// <summary>
        /// Null if default value
        /// </summary>
        [DbFunction("Edm", "NullIf")]
        public static byte[] DbNullIf(this byte[] value, byte[] defaultValue)
        {
            return value == defaultValue ? null : value;
        }
    }
}
