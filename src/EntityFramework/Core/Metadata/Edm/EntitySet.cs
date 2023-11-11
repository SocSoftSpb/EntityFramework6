// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Metadata.Edm
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity.Core.Common.CommandTrees;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity.Core.Common.Utils;
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm.Provider;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Utilities;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    
    using ITreeGenerator = System.Data.Entity.Core.Query.PlanCompiler.ITreeGenerator;
    using PlanCompiler = System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler;
    using OpCopier = System.Data.Entity.Core.Query.InternalTrees.OpCopier;
    using Node = System.Data.Entity.Core.Query.InternalTrees.Node;
    using OpType = System.Data.Entity.Core.Query.InternalTrees.OpType;
    using Command = System.Data.Entity.Core.Query.InternalTrees.Command;

    /// <summary>
    /// Represents a particular usage of a structure defined in EntityType. In the conceptual-model, this represents a set that can 
    /// query and persist entities. In the store-model it represents a table. 
    /// From a store-space model-convention it can be used to configure
    /// table name with <see cref="EntitySetBase.Table"/> property and table schema with <see cref="EntitySetBase.Schema"/> property.
    /// </summary>
    public class EntitySet : EntitySetBase
    {
        internal EntitySet()
        {
        }

        // <summary>
        // The constructor for constructing the EntitySet with a given name and an entity type
        // </summary>
        // <param name="name"> The name of the EntitySet </param>
        // <param name="schema"> The db schema </param>
        // <param name="table"> The db table </param>
        // <param name="definingQuery"> The provider specific query that should be used to retrieve the EntitySet </param>
        // <param name="entityType"> The entity type of the entities that this entity set type contains </param>
        // <exception cref="System.ArgumentNullException">Thrown if the argument name or entityType is null</exception>
        internal EntitySet(string name, string schema, string table, string definingQuery, EntityType entityType)
            : base(name, schema, table, definingQuery, entityType)
        {
        }

        private ReadOnlyCollection<Tuple<AssociationSet, ReferentialConstraint>> _foreignKeyDependents;
        private ReadOnlyCollection<Tuple<AssociationSet, ReferentialConstraint>> _foreignKeyPrincipals;
        private ReadOnlyCollection<AssociationSet> _associationSets;
        private volatile bool _hasForeignKeyRelationships;
        private volatile bool _hasIndependentRelationships;

        internal DynamicEntitySetMapper DynamicEntitySetMapper { get; set; }

        /// <summary>
        /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.EntitySet" />.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
        /// <see
        ///     cref="T:System.Data.Entity.Core.Metadata.Edm.EntitySet" />
        /// .
        /// </returns>
        public override BuiltInTypeKind BuiltInTypeKind
        {
            get { return BuiltInTypeKind.EntitySet; }
        }

        /// <summary>
        /// Gets the entity type of this <see cref="T:System.Data.Entity.Core.Metadata.Edm.EntitySet" />.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.EntityType" /> object that represents the entity type of this
        /// <see
        ///     cref="T:System.Data.Entity.Core.Metadata.Edm.EntitySet" />
        /// .
        /// </returns>
        public new virtual EntityType ElementType
        {
            get { return (EntityType)base.ElementType; }
        }

        // <summary>
        // Returns the associations and constraints where "this" EntitySet particpates as the Principal end.
        // From the results of this list, you can retrieve the Dependent IRelatedEnds
        // </summary>
        internal ReadOnlyCollection<Tuple<AssociationSet, ReferentialConstraint>> ForeignKeyDependents
        {
            get
            {
                if (_foreignKeyDependents == null)
                {
                    InitializeForeignKeyLists();
                }
                return _foreignKeyDependents;
            }
        }

        // <summary>
        // Returns the associations and constraints where "this" EntitySet particpates as the Dependent end.
        // From the results of this list, you can retrieve the Principal IRelatedEnds
        // </summary>
        internal ReadOnlyCollection<Tuple<AssociationSet, ReferentialConstraint>> ForeignKeyPrincipals
        {
            get
            {
                if (_foreignKeyPrincipals == null)
                {
                    InitializeForeignKeyLists();
                }
                return _foreignKeyPrincipals;
            }
        }

        internal ReadOnlyCollection<AssociationSet> AssociationSets
        {
            get
            {
                if (_foreignKeyPrincipals == null)
                {
                    InitializeForeignKeyLists();
                }
                return _associationSets;
            }
        }

        // <summary>
        // True if this entity set participates in any foreign key relationships, otherwise false.
        // </summary>
        internal bool HasForeignKeyRelationships
        {
            get
            {
                if (_foreignKeyPrincipals == null)
                {
                    InitializeForeignKeyLists();
                }
                return _hasForeignKeyRelationships;
            }
        }

        // <summary>
        // True if this entity set participates in any independent relationships, otherwise false.
        // </summary>
        internal bool HasIndependentRelationships
        {
            get
            {
                if (_foreignKeyPrincipals == null)
                {
                    InitializeForeignKeyLists();
                }
                return _hasIndependentRelationships;
            }
        }

        private void InitializeForeignKeyLists()
        {
            var dependents = new List<Tuple<AssociationSet, ReferentialConstraint>>();
            var principals = new List<Tuple<AssociationSet, ReferentialConstraint>>();
            var foundFkRelationship = false;
            var foundIndependentRelationship = false;
            var associationsForEntitySet = new ReadOnlyCollection<AssociationSet>(MetadataHelper.GetAssociationsForEntitySet(this));
            foreach (var associationSet in associationsForEntitySet)
            {
                if (associationSet.ElementType.IsForeignKey)
                {
                    foundFkRelationship = true;
                    Debug.Assert(associationSet.ElementType.ReferentialConstraints.Count == 1, "Expected exactly one constraint for FK");
                    var constraint = associationSet.ElementType.ReferentialConstraints[0];
                    if (constraint.ToRole.GetEntityType().IsAssignableFrom(ElementType)
                        ||
                        ElementType.IsAssignableFrom(constraint.ToRole.GetEntityType()))
                    {
                        // Dependents
                        dependents.Add(new Tuple<AssociationSet, ReferentialConstraint>(associationSet, constraint));
                    }
                    if (constraint.FromRole.GetEntityType().IsAssignableFrom(ElementType)
                        ||
                        ElementType.IsAssignableFrom(constraint.FromRole.GetEntityType()))
                    {
                        // Principals
                        principals.Add(new Tuple<AssociationSet, ReferentialConstraint>(associationSet, constraint));
                    }
                }
                else
                {
                    foundIndependentRelationship = true;
                }
            }

            _hasForeignKeyRelationships = foundFkRelationship;
            _hasIndependentRelationships = foundIndependentRelationship;

            var readOnlyDependents = new ReadOnlyCollection<Tuple<AssociationSet, ReferentialConstraint>>(dependents);
            var readOnlyPrincipals = new ReadOnlyCollection<Tuple<AssociationSet, ReferentialConstraint>>(principals);

            Interlocked.CompareExchange(ref _foreignKeyDependents, readOnlyDependents, null);
            Interlocked.CompareExchange(ref _foreignKeyPrincipals, readOnlyPrincipals, null);
            Interlocked.CompareExchange(ref _associationSets, associationsForEntitySet, null);
        }

        /// <summary>
        /// The factory method for constructing the EntitySet object.
        /// </summary>
        /// <param name="name">The name of the EntitySet.</param>
        /// <param name="schema">The db schema. Can be null.</param>
        /// <param name="table">The db table. Can be null.</param>
        /// <param name="definingQuery">
        /// The provider specific query that should be used to retrieve data for this EntitySet. Can be null.
        /// </param>
        /// <param name="entityType">The entity type of the entities that this entity set type contains.</param>
        /// <param name="metadataProperties">
        /// Metadata properties that will be added to the newly created EntitySet. Can be null.
        /// </param>
        /// <returns>The EntitySet object.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the name argument is null or empty string.</exception>
        /// <remarks>The newly created EntitySet will be read only.</remarks>
        public static EntitySet Create(
            string name, string schema, string table, string definingQuery, EntityType entityType,
            IEnumerable<MetadataProperty> metadataProperties)
        {
            Check.NotEmpty(name, "name");
            Check.NotNull(entityType, "entityType");

            var entitySet = new EntitySet(name, schema, table, definingQuery, entityType);

            if (metadataProperties != null)
            {
                entitySet.AddMetadataProperties(metadataProperties);
            }

            entitySet.SetReadOnly();
            return entitySet;
        }

        internal ObjectTypeMapping GetDynamicObjectMapping()
        {
            return DynamicEntitySetMapper.GetDynamicObjectMapping();
        }

        internal bool AllowDynamicPlanCaching()
        {
            return DynamicEntitySetMapper.AllowDynamicPlanCaching();
        }
    }

    internal class DynamicEntitySetMapper
    {
        const string DynamicEntityTypeNamespace = "DynamicNamespace_S";

        private readonly EntitySet _entitySet;
        private readonly DynamicEntitySetOptions _options;
        private readonly Type _clrType;
        private readonly MetadataWorkspace _metadataWorkspace;
        private volatile ClrEntityType _clrEntityType;
        private volatile ObjectTypeMapping _dynamicObjectMapping;
        private Node _internalTree;

        public DynamicEntitySetMapper(EntitySet entitySet, DynamicEntitySetOptions options, Type clrType, MetadataWorkspace metadataWorkspace)
        {
            _metadataWorkspace = metadataWorkspace;
            _entitySet = entitySet;
            _options = options;
            _clrType = clrType;
        }

        public EntitySet StoreEntitySet { get; private set; }

        public void ProcessMapping()
        {
            var storeItemCollection = (StoreItemCollection)_metadataWorkspace.GetItemCollection(DataSpace.SSpace);
            var containerSSpase = GetContainer(storeItemCollection);
            if (containerSSpase == null)
                throw new InvalidOperationException("Can't get container in SSpace.");

            var hasDifferentColumns = _options.Columns.Any(e => e.Property.Name != e.ColumnName); 

            _entitySet.DynamicEntitySetMapper = this;
            var typeCSpace = _entitySet.ElementType;
            typeCSpace.DynamicEntitySet = _entitySet;
            
            if (hasDifferentColumns)
            {
                var containerCSpase = GetContainer(_metadataWorkspace.GetItemCollection(DataSpace.CSpace));
                _entitySet.ChangeEntityContainerWithoutCollectionFixup(containerCSpase);
                string[] keyMemberNames = null;
                if (_options.KeyMemberNames != null)
                    keyMemberNames = new string[_options.KeyMemberNames.Length];
                var lstEdmProperties = new List<EdmMember>(typeCSpace.Properties.Count);
                var iKeyMember = 0;
                
                foreach (var cProperty in typeCSpace.Properties)
                {
                    var colOption = _options.Columns.Single(e => e.Property.Name == cProperty.Name);
                    var underlyingTypeUsage = TypeUsage.Create(cProperty.UnderlyingPrimitiveType, cProperty.TypeUsage.Facets);
                    var storeTypeUsage = storeItemCollection.ProviderManifest.GetStoreType(underlyingTypeUsage);
                    var sProperty = new EdmProperty(colOption.ColumnName, storeTypeUsage)
                    {
                        Nullable = cProperty.Nullable
                    };
                    lstEdmProperties.Add(sProperty);
                    
                    if (_options.KeyMemberNames != null && keyMemberNames != null && _options.KeyMemberNames.Contains(cProperty.Name))
                    {
                        keyMemberNames[iKeyMember++] = sProperty.Name;
                    }
                }
                
                var typeSSpace = new EntityType(typeCSpace.Name, DynamicEntityTypeNamespace, DataSpace.SSpace, keyMemberNames, lstEdmProperties);
                var setSSpace = new EntitySet(_entitySet.Name, _entitySet.Schema, _entitySet.Table, _entitySet.DefiningQuery, typeSSpace)
                {
                    Database = _entitySet.Database,
                    DynamicEntitySetMapper = this
                };
                typeSSpace.DynamicEntitySet = setSSpace;

                setSSpace.ChangeEntityContainerWithoutCollectionFixup(containerSSpase);
                setSSpace.SetReadOnly();
                StoreEntitySet = setSSpace;
                
                _entitySet.ChangeEntityContainerWithoutCollectionFixup(containerCSpase);
            }
            else
            {
                _entitySet.ChangeEntityContainerWithoutCollectionFixup(containerSSpase);
            }
            
            _entitySet.SetReadOnly();
        }

        private Node BuildInternalTree()
        {
            var sourceType = StoreEntitySet.ElementType;
            var input = StoreEntitySet.Scan().BindAs("row");
            var resultType = TypeUsage.Create(_entitySet.ElementType);
            var projection = resultType.New(sourceType.Properties.Select(p => input.Variable.Property(p)));

            var query = input.Project(projection);

            var commandTree = DbQueryCommandTree.FromValidExpression(
                _metadataWorkspace, TargetPerspective.TargetPerspectiveDataSpace, query,
                useDatabaseNullSemantics: true, disableFilterOverProjectionSimplificationForCustomFunctions: false);

            // Convert this into an ITree first
            var itree = ITreeGenerator.Generate(commandTree, discriminatorMap: null);
            // Pull out the root physical project-op, and copy this itree into our own itree
            PlanCompiler.Assert(
                itree.Root.Op.OpType == OpType.PhysicalProject,
                "Expected a physical projectOp at the root of the tree - found " + itree.Root.Op.OpType);
            // #554756: VarVec enumerators are not cached on the shared Command instance.
            itree.DisableVarVecEnumCaching();
            return itree.Root.Child0;
        }
        
        internal Node GetInternalTree(Command targetIqtCommand, TableHints hints)
        {
            var internalTree = _internalTree;
            if (internalTree == null)
            {
                Interlocked.CompareExchange(ref _internalTree, BuildInternalTree(), null);
                internalTree = _internalTree;
            }
            Debug.Assert(internalTree != null, "m_internalTreeNode != null");
            return OpCopier.Copy(targetIqtCommand, internalTree, hints);
        }

        private EntityContainer GetContainer(ItemCollection itemCollection)
        {
            var container = itemCollection.GetItems<EntityContainer>().FirstOrDefault();
            if (container == null)
                throw new InvalidOperationException($"Can't get container in DataSpace {itemCollection.DataSpace}.");
            return container;
        }

        public ObjectTypeMapping GetDynamicObjectMapping()
        {
            return _dynamicObjectMapping ?? (_dynamicObjectMapping = CreateDynamicObjectMapping());
        }

        private ClrEntityType GetClrEntityType()
        {
            return _clrEntityType ?? (_clrEntityType = CreateClrEntityType());
        }

        private ClrEntityType CreateClrEntityType()
        {
            var clrEntityType = new ClrEntityType(_clrType, _clrType.Namespace, _clrType.Name);
            foreach (var optionsColumn in _options.Columns)
            {
                var property = optionsColumn.Property;
                if (!TryGetPrimitiveType(property.PropertyType, out var primitiveType))
                    throw new InvalidOperationException($"Can't get primitive type for property: {property.Name}.");

                var member = new EdmProperty(property.Name, TypeUsage.Create(
                        primitiveType, new FacetValues
                        {
                            Nullable = optionsColumn.IsNullable
                        }),
                    property, _clrType);

                clrEntityType.AddMember(member);
            }

            return clrEntityType;
        }

        private ObjectTypeMapping CreateDynamicObjectMapping()
        {
            var clrEntityType = GetClrEntityType();
            var entityType = _entitySet.ElementType;
            var mapping = new ObjectTypeMapping(clrEntityType, entityType);

            foreach (var optionsColumn in _options.Columns)
            {
                var clrProperty = clrEntityType.Properties[optionsColumn.Property.Name];
                var edmProperty = entityType.Properties[optionsColumn.Property.Name];

                mapping.AddMemberMap(new ObjectPropertyMapping(edmProperty, clrProperty));
            }

            return mapping;
        }

        protected static bool TryGetPrimitiveType(Type type, out PrimitiveType primitiveType)
        {
            return ClrProviderManifest.Instance.TryGetPrimitiveType(Nullable.GetUnderlyingType(type) ?? type, out primitiveType);
        }

        public bool AllowDynamicPlanCaching()
        {
            return _options.UniqueSetName != null;
        }

        public Dictionary<string, string> GetDifferentColumnMappings()
        {
            if (StoreEntitySet == null)
                return null;

            Dictionary<string, string> res = null;
            foreach (var col in _options.Columns)
            {
                if (col.Property.Name != col.ColumnName)
                    (res ?? (res = new Dictionary<string, string>())).Add(col.Property.Name, col.ColumnName);
            }

            return res;
        }
    }
}
