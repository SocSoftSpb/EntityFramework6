// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration
{
    public class VectorParameterConfiguration
    {
        private readonly Configuration.Mapping.VectorParameterConfiguration _configuration;

        public VectorParameterConfiguration(Configuration.Mapping.VectorParameterConfiguration configuration)
        {
            _configuration = configuration;
        }

        public VectorParameterConfiguration HasStoreType(string schema, string name)
        {
            _configuration.StoreTypeSchema = schema;
            _configuration.StoreTypeName = name;
            
            return this;
        }
        
        public VectorParameterConfiguration HasColumnName(string columnName)
        {
            _configuration.ColumnName = columnName;
            
            return this;
        }
    }
}
