// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Edm
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Utilities;

    public static class VectorParameterTypeExtensions
    {
        public static Type GetClrType(this VectorParameterType vectorParameterType)
        {
            DebugCheck.NotNull(vectorParameterType);

            return vectorParameterType.Annotations.GetClrType();
        }
    }
}
