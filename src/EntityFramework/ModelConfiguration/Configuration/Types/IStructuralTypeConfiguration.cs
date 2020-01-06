// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration
{
    using System.Data.Entity.ModelConfiguration.Configuration.Types;

    /// <summary>
    /// Abstraction for <see cref="StructuralTypeConfiguration{TStructuralType}"/> API
    /// </summary>
    public interface IStructuralTypeConfiguration
    {
        /// <summary>
        /// Get internal non-generic configuration
        /// </summary>
        StructuralTypeConfiguration InternalConfiguration { get; }
    }
}
