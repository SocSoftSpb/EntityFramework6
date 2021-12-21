// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Metadata.Edm
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Core.Metadata.Edm.Provider;
    using System.Data.Entity.Utilities;
    using System.Diagnostics;
    using System.Linq;

    public class VectorParameterType : SimpleType
    {
        private static readonly ReadOnlyCollection<FacetDescription> _facetDescriptions;

        public const string WrapperFunctionName = "__VectorParameterWrapper__";

        static VectorParameterType()
        {
            var arr = GetGeneralFacetDescriptions().ToArray();
            for (var i = 0; i < arr.Length; i++)
            {
                var facetDescription = arr[i];
                if (facetDescription.FacetName == DbProviderManifest.NullableFacetName)
                {
                    arr[i] = new FacetDescription(
                        facetDescription.FacetName, facetDescription.FacetType,
                        facetDescription.MinValue, facetDescription.MaxValue, defaultValue: false);
                }
            }

            _facetDescriptions = new ReadOnlyCollection<FacetDescription>(arr);
        }
        
        public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.VectorParameterType;
        
        [MetadataProperty(BuiltInTypeKind.PrimitiveType, false)]
        public PrimitiveType ElementType { get; }
        
        public TypeUsage ElementTypeUsage { get; }
        
        public VectorParameterType(PrimitiveType elementType, string namespaceName, DataSpace dataSpace)
            : base(BuildName(elementType.Name), namespaceName, dataSpace)
        {
            ElementType = elementType;
            ElementTypeUsage = TypeUsage.Create(elementType);
        }
        
        internal VectorParameterType(Type clrType)
            :
            base(BuildClrName(clrType, out var nspName), nspName, DataSpace.OSpace)
        {
            DebugCheck.NotNull(clrType);
            Debug.Assert(clrType.IsVectorParameter(), "VectorParameter type expected");

            var underType = clrType.GetGenericArguments()[0];
            
            ClrProviderManifest.Instance.TryGetPrimitiveType(underType, out var elementType);
            Debug.Assert(elementType != null, "only primitive types expected here.");

            ElementType = elementType;
            ElementTypeUsage = TypeUsage.Create(elementType);
        }

        internal static string BuildClrName(Type clrType, out string namespaceName)
        {
            namespaceName = clrType.NestingNamespace() ?? string.Empty;
            var retVal = clrType.FullName;
            if (retVal != null && !string.IsNullOrEmpty(namespaceName))
                retVal = retVal.Substring(namespaceName.Length + 1);

            return retVal;
        }

        public static string BuildName(string primitiveTypeName)
        {
            return "VectorOf" + primitiveTypeName;
        }

        internal override IEnumerable<FacetDescription> GetAssociatedFacetDescriptions()
        {
            return _facetDescriptions;
        }
    }
}
