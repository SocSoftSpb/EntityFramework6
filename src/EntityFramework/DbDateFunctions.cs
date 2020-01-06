// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace System.Data.Entity
{
    /// <summary>
    /// DateTime functions
    /// </summary>
    public static class DbDateFunctions
    {
        /// <summary>
        /// Get Year of date
        /// </summary>
        public static int? Year(this DateTime? value) => value?.Year;

        /// <summary>
        /// Get Month of date
        /// </summary>
        public static int? Month(this DateTime? value) => value?.Month;

        /// <summary>
        /// Get Day of date
        /// </summary>
        public static int? Day(this DateTime? value) => value?.Day;

        /// <summary>
        /// Get Hour of date
        /// </summary>
        public static int? Hour(this DateTime? value) => value?.Hour;

        /// <summary>
        /// Get Minute of date
        /// </summary>
        public static int? Minute(this DateTime? value) => value?.Minute;

        /// <summary>
        /// Get Second of date
        /// </summary>
        public static int? Second(this DateTime? value) => value?.Second;

        /// <summary>
        /// Get Millisecond of date
        /// </summary>
        public static int? Millisecond(this DateTime? value) => value?.Millisecond;

    }
}
