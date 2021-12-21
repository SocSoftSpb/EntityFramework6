// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Mapping
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Diagnostics;

    public class VectorParameterTypeMapping : MappingItem
    {
        private readonly EntityContainerMapping _containerMapping;
        private object _providerServicesMetadata;

        public VectorParameterTypeMapping(EntityContainerMapping containerMapping, VectorParameterType vectorParameterType)
        {
            _containerMapping = containerMapping;
            VectorParameterType = vectorParameterType;
        }

        public VectorParameterType VectorParameterType { get; }
        
        public string StoreTypeSchema { get; private set; }
        
        public string StoreTypeName { get; private set; }
        
        public string ColumnName { get; private set; }
        
        internal EdmFunction WrapperFunction { get; private set; }

        internal void Configure(string storeTypeSchema, string storeTypeName, string columnName)
        {
            Debug.Assert(!IsReadOnly);
            
            StoreTypeSchema = storeTypeSchema;
            StoreTypeName = storeTypeName;
            ColumnName = columnName;

            WrapperFunction = CreateWrapperFunction();
        }
        
        private EdmFunction CreateWrapperFunction()
        {
            var elementType = VectorParameterType.ElementTypeUsage;

            var rowType = new RowType(new[] { new EdmProperty(ColumnName ?? "Value", elementType) });
            var returnType = TypeUsage.Create(rowType.GetCollectionType());
            var returnParameter = new FunctionParameter(EdmConstants.ReturnType, returnType, ParameterMode.ReturnValue, false);
            
            var functionPayload = new EdmFunctionPayload
            {
                IsBuiltIn = true,
                IsFromProviderManifest = true,
                ReturnParameters = new[] { returnParameter },
                Parameters = new[] {new FunctionParameter("p0", TypeUsage.Create(VectorParameterType), ParameterMode.In, false)}
            };

            var function
                = new EdmFunction(
                    VectorParameterType.WrapperFunctionName,
                    VectorParameterType.NamespaceName,
                    DataSpace.SSpace,
                    functionPayload);
            
            function.SetReadOnly();

            return function;
        }

        public bool HasStoreMapping()
        {
            return StoreTypeSchema != null || StoreTypeName != null || ColumnName != null;
        }

        public object ProviderServicesMetadata
        {
            get => _providerServicesMetadata;
            set => _providerServicesMetadata = value;
        }
    }
}
