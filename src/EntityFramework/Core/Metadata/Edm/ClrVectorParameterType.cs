// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Metadata.Edm
{
    using System.Data.Entity.Utilities;
    using System.Diagnostics;

    public class ClrVectorParameterType : VectorParameterType
    {
        private readonly Type _type;
        private readonly string _cspaceTypeName;

        internal ClrVectorParameterType(Type clrType, string cspaceNamespaceName, string cspaceTypeName)
            : base(clrType)
        {
            DebugCheck.NotNull(clrType);
            DebugCheck.NotEmpty(cspaceNamespaceName);
            DebugCheck.NotEmpty(cspaceTypeName);
            Debug.Assert(clrType.IsVectorParameter(), "vector parameter type expected");

            _type = clrType;
            _cspaceTypeName = cspaceNamespaceName + "." + cspaceTypeName;
        }
        
        internal override Type ClrType
        {
            get { return _type; }
        }
        
        internal string CSpaceTypeName
        {
            get { return _cspaceTypeName; }
        }
    }
}
