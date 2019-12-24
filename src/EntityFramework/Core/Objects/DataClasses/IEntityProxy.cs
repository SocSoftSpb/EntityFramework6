// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Objects.DataClasses
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Marker and implementor for proxy types
    /// </summary>
    public interface IEntityProxy
    {
        /// <summary>
        /// Get EntityWrapper for proxy
        /// </summary>
        /// <returns>EntityWrapper for proxy</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IPublicEntityWrapper GetWrapper();
    }
}
