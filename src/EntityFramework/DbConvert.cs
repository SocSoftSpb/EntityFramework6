// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace System.Data.Entity 
{
    using System.Data.Entity.Resources;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Type Conversion Functions
    /// </summary>
    public static class DbConvert
    {
        /// <summary>
        /// Convert any type to DateTime
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        public static DateTime? ToDateTime<T>(T value)
        {
            throw new NotSupportedException(Strings.ELinq_DbFunctionDirectCall);
        }
    }
}
