// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Objects
{
    /// <summary>
    /// Represents a cache of compiled query plans for a single query.
    /// </summary>
    /// <remarks>
    /// This interface is used internally by the Entity Framework to manage a cache of compiled query plans for a single query.
    /// </remarks>
    public interface ISingleCompiledPlanCache
    {
        /// <summary>
        /// Gets the stored compiled query plan.
        /// </summary>
        /// <returns></returns>
        object Get();

        /// <summary>
        /// Gets the stored compiled query plan, or adds a new plan to the cache if one does not already exist.
        /// </summary>
        /// <param name="entry">The compiled query plan to add to the cache if one does not already exist.</param>
        /// <param name="foundEntry">The compiled query plan that was found in the cache, or the one that was added.</param>
        /// <returns>False if a new plan was added to the cache; True if an existing plan was found.</returns>
        bool TryLookupAndAdd(object entry, out object foundEntry);
    }
}
