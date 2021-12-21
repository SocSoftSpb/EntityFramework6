// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration.Mapping
{
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.ModelConfiguration.Edm;

    public class VectorParameterConfiguration : ConfigurationBase
    {
        private readonly Type _elementType;

        public VectorParameterConfiguration(Type elementType)
        {
            _elementType = elementType;
        }

        private VectorParameterConfiguration(VectorParameterConfiguration other)
            : this(other._elementType)
        {
            StoreTypeSchema = other.StoreTypeSchema;
            StoreTypeName = other.StoreTypeName;
            ColumnName = other.ColumnName;
        }

        public Type ElementType => _elementType;

        public VectorParameterConfiguration Clone()
        {
            return new VectorParameterConfiguration(this);
        }
        
        public string StoreTypeSchema { get; set; }
        
        public string StoreTypeName { get; set; }
        
        public string ColumnName { get; set; }

        internal void Configure(DbDatabaseMapping databaseMapping, VectorParameterTypeMapping vectorParameterTypeMapping)
        {
            vectorParameterTypeMapping.Configure(StoreTypeSchema, StoreTypeName, ColumnName);
        }
    }
}
