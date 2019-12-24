// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Objects.DataClasses
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Fast access to <see cref="ObjectStateEntry"/> for proxy types
    /// </summary>
    public interface IPublicEntityWrapper
    {
        /// <summary>
        /// If this IEntityWrapper is tracked, accesses the ObjectStateEntry that is used in the state manager
        /// </summary>
        /// <returns>ObjectStateEntry for this wrapper</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ObjectStateEntry GetObjectStateEntry();

    }
}
