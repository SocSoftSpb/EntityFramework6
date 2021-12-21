// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.SchemaObjectModel
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Resources;
    using System.Data.Entity.Utilities;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml;

    internal class SchemaVectorParameterType : SchemaType
    {
        private SchemaType _elementType;
        private string _unresolvedElementTypeName;

        // <summary>
        // Returns underlying type for this enum.
        // </summary>
        public SchemaType ElementType
        {
            get
            {
                Debug.Assert(_elementType != null, "The type has not been resolved yet");

                return _elementType;
            }
        }

        public SchemaVectorParameterType(Schema parentElement)
            : base(parentElement)
        {
            if (Schema.DataModel == SchemaDataModelOption.EntityDataModel)
            {
                OtherContent.Add(Schema.SchemaSource);
            }
        }

        protected override bool HandleElement(XmlReader reader)
        {
            DebugCheck.NotNull(reader);

            if (!base.HandleElement(reader))
            {
                if (CanHandleElement(reader, XmlConstants.ValueAnnotation))
                {
                    // EF does not support this EDM 3.0 element, so ignore it.
                    SkipElement(reader);
                    return true;
                }
                else if (CanHandleElement(reader, XmlConstants.TypeAnnotation))
                {
                    // EF does not support this EDM 3.0 element, so ignore it.
                    SkipElement(reader);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        
        protected override bool HandleAttribute(XmlReader reader)
        {
            DebugCheck.NotNull(reader);

            if (!base.HandleAttribute(reader))
            {
                if (CanHandleAttribute(reader, XmlConstants.ElementType))
                {
                    Utils.GetDottedName(Schema, reader, out _unresolvedElementTypeName);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        
        internal override void ResolveTopLevelNames()
        {
            // if the underlying type was not specified in the CSDL we use int by default
            if (_unresolvedElementTypeName == null)
            {
            }
            else
            {
                Debug.Assert(_unresolvedElementTypeName.Length != 0);
                Schema.ResolveTypeName(this, _unresolvedElementTypeName, out _elementType);
            }
        }
        
        internal override void Validate()
        {
            base.Validate();

            var elType = ElementType as ScalarType;

            if (elType == null)
            {
                AddError(
                    ErrorCode.InvalidVectorParameterElementType,
                    EdmSchemaErrorSeverity.Error,
                    Strings.InvalidVectorParameterElementType);
            }
        }
    }
}
