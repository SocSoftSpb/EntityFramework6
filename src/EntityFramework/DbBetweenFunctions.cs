// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity
{
    using System.Collections.Generic;

    /// <summary>
    /// BETWEEN functions
    /// </summary>
    public static class DbBetweenFunctions
    {
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this byte? value, byte? begin, byte? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this byte value, byte begin, byte end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this short? value, short? begin, short? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this short value, short begin, short end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this int? value, int? begin, int? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this int value, int begin, int end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this long? value, long? begin, long? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this long value, long begin, long end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this decimal? value, decimal? begin, decimal? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this decimal value, decimal begin, decimal end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this double? value, double? begin, double? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this double value, double begin, double end)
        {
            return value >= begin && value <= end;
        }

        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this float? value, float? begin, float? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this float value, float begin, float end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this DateTime? value, DateTime? begin, DateTime? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this DateTime value, DateTime begin, DateTime end)
        {
            return value >= begin && value <= end;
        }

        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this DateTimeOffset? value, DateTimeOffset? begin, DateTimeOffset? end)
        {
            return value >= begin && value <= end;
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this DateTimeOffset value, DateTimeOffset begin, DateTimeOffset end)
        {
            return value >= begin && value <= end;
        }

        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this Guid? value, Guid? begin, Guid? end)
        {
            throw new InvalidOperationException("This function can only be invoked from LINQ to Entities. Guid comparision is DB-specific.");
        }
        
        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this Guid value, Guid begin, Guid end)
        {
            throw new InvalidOperationException("This function can only be invoked from LINQ to Entities. Guid comparision is DB-specific.");
        }

        /// <summary>
        /// Test if <paramref name="value"/> &lt;= <paramref name="begin"/> AND <paramref name="value"/> &gt;= <paramref name="end"/>
        /// </summary>
        [DbFunction("Edm", "Between")]
        public static bool Between(this string value, string begin, string end)
        {
            var comparer = StringComparer.Ordinal;
            return comparer.Compare(value, begin) >= 0 && comparer.Compare(value, end) <= 0;
        }
    }
}
