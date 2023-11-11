namespace System.Data.Entity.Core.Common.CommandTrees
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity.Core.EntityClient.Internal;
    using System.Data.Entity.Core.Mapping;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Core.Objects.ELinq;
    using System.Data.Entity.Core.Query.InternalTrees;
    using System.Threading;

    public enum DbDmlOperationKind
    {
        None = 0,
        Delete = 1,
        Update = 2,
        Insert = 3
    }

    public abstract class DbDmlOperation
    {
        protected DbDmlOperation(DbDmlOperationKind kind, EntitySet targetEntitySet, bool withRowCount)
        {
            Kind = kind;
            TargetEntitySet = targetEntitySet;
            TargetDataSpace = TargetEntitySet.ElementType.DataSpace;
            WithRowCount = withRowCount;
        }

        public DbDmlOperationKind Kind { get; }

        /// <summary>
        /// Must returns rows count
        /// </summary>
        public bool WithRowCount { get; }

        /// <summary>
        /// Target EntitySet of this operation (Storage)
        /// </summary>
        public EntitySet TargetEntitySet { get; }
        
        /// <summary>
        /// Target EntitySet Element DataSpace
        /// </summary>
        public DataSpace TargetDataSpace { get; }

        public abstract bool CanConstructEntityTypeInExpressionConverter(Type entityType);
        internal abstract void AssignMapping(ColumnMap columnMap, DbQueryCommandTree commandTree);

        public virtual string GetCacheKey() => WithRowCount ? Kind + nameof(WithRowCount) : Kind.ToString();

        internal ColumnMap CreateColumnMap()
        {
            var primUsage = TypeHelpers.GetLiteralTypeUsage(PrimitiveTypeKind.Int32);
            var collectionTyp = TypeHelpers.CreateCollectionTypeUsage(primUsage);

            var colMap = new ScalarColumnMap(primUsage, "Value", 0, 0);
            return new SimpleCollectionColumnMap(collectionTyp, "Value", colMap, null, null);
        }
    }

    public sealed class DbDmlDeleteOperation : DbDmlOperation
    {
        internal DbDmlDeleteOperation(EntitySet targetEntitySet, bool withRowCount, int limit)
            : base(DbDmlOperationKind.Delete, targetEntitySet, withRowCount)
        {
            Limit = limit;
        }

        public int Limit { get; }
        
        public override bool CanConstructEntityTypeInExpressionConverter(Type entityType)
        {
            return false;
        }

        internal override void AssignMapping(ColumnMap columnMap, DbQueryCommandTree commandTree)
        {
        }
    }

    public abstract class DbDmlUpInsOperationBase : DbDmlOperation
    {
        private bool _isUnderTranslation = true;

        public Type ClrEntityType { get; }
        
        /// <summary>
        /// Mapping for column names different from Property Names. Key - CLR PropertyName, Value - Db Column Name. Contains Only changed name.
        /// Can be null if all property names equals to column names 
        /// </summary>
        public Dictionary<string, string> ColNameMappings { get; }

        public DmlColumnMapping ColumnMap { get; private set; }

        public bool IsUnderTranslation
        {
            get => _isUnderTranslation;
            set
            {
                if (!_isUnderTranslation && value)
                    throw new InvalidOperationException("Can't switch again to UnderTranslation.");
                _isUnderTranslation = value;
            }
        }

        protected DbDmlUpInsOperationBase(DbDmlOperationKind kind, EntitySet targetEntitySet, Type clrEntityType, bool withRowCount, Dictionary<string, string> colNameMappings)
            : base(kind, targetEntitySet, withRowCount)
        {
            ClrEntityType = clrEntityType;
            ColNameMappings = colNameMappings;
        }

        public sealed override bool CanConstructEntityTypeInExpressionConverter(Type entityType)
        {
            return IsUnderTranslation && (ClrEntityType.IsAbstract ? entityType.IsSubclassOf(ClrEntityType) : entityType == ClrEntityType);
        }

        internal override void AssignMapping(ColumnMap columnMap, DbQueryCommandTree commandTree)
        {
            if (!(columnMap is CollectionColumnMap colMap))
                throw new InvalidOperationException("CollectionColumnMap expected.");

            if (!(colMap.Element is StructuredColumnMap structuredColumnMap))
                throw new InvalidOperationException("StructuredColumnMap expected.");

            var dmlMap = new DmlColumnMapping(structuredColumnMap.NullSentinel is ScalarColumnMap nsm ? nsm.ColumnPos : -1, structuredColumnMap.Properties.Length);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < structuredColumnMap.Properties.Length; index++)
            {
                var map = structuredColumnMap.Properties[index];
                if (!(map is ScalarColumnMap scalarColumnMap))
                    throw new InvalidOperationException("ScalarColumnMap expected.");

                var targetColumnName = (ColNameMappings != null && ColNameMappings.TryGetValue(scalarColumnMap.Name, out var colName)) ? colName : scalarColumnMap.Name;
                dmlMap.Mappings[index] = new DmlColumnMap(scalarColumnMap.ColumnPos, scalarColumnMap.Name, targetColumnName);
            }

            ColumnMap = dmlMap;
        }
    }

    public sealed class DbDmlUpdateOperation : DbDmlUpInsOperationBase
    {
        internal DbDmlUpdateOperation(EntitySet targetEntitySet, Type clrEntityType, bool withRowCount, int limit, Dictionary<string, string> colNameMappings)
            : base(DbDmlOperationKind.Update, targetEntitySet, clrEntityType, withRowCount, colNameMappings)
        {
            Limit = limit;
        }

        public int Limit { get; }

        public override string GetCacheKey()
        {
            var key = base.GetCacheKey();
            if (Limit >= 0)
                key = string.Concat(key, " LIMIT ", Limit.ToString());
            return key;
        }
    }

    public sealed class DbDmlInsertOperation : DbDmlUpInsOperationBase
    {
        private IList<EdmProperty> _unmappedRequiredProperties;
#if NET40
        private static readonly EdmProperty[] _emptyEdmProperty = new EdmProperty[0];
#else
        private static readonly EdmProperty[] _emptyEdmProperty = Array.Empty<EdmProperty>();
#endif

        public ValueConditionMapping[] Discriminators { get; }
        public ObjectQuery FromObjectQuery { get; }
        public ObjectQueryColumnMap[] FromQueryMapping { get; }
        public DbCommand FromStoreCommand { get; }
        internal IEnumerable<Tuple<ObjectParameter, QueryParameterExpression>> LinqParameters { get; }
        
        internal DbDmlInsertOperation(EntitySet targetEntitySet, Type clrEntityType, bool withRowCount, ValueConditionMapping[] discriminators, 
            Dictionary<string, string> colNameMappings, ObjectQuery fromObjectQuery)
            : base(DbDmlOperationKind.Insert, targetEntitySet, clrEntityType, withRowCount, colNameMappings)
        {
            Discriminators = discriminators;
            FromObjectQuery = fromObjectQuery;

            if (fromObjectQuery != null)
            {
                if (!(fromObjectQuery.QueryState is ELinqQueryState linqState))
                    throw new NotSupportedException("Insert operation supporter only for Linq-to-Entities.");
                
                var plan = fromObjectQuery.QueryState.GetExecutionPlan(MergeOption.NoTracking);
                var commandDefinition = (EntityCommandDefinition)(plan.CommandDefinition);
                FromQueryMapping =  commandDefinition.GetProjectionMapping();
                FromStoreCommand = commandDefinition.CreateStoreCommand();
                LinqParameters = linqState.GetLinqParameters(plan);
            }
        }

        public override string GetCacheKey()
        {
            var key = base.GetCacheKey();
            if (Discriminators != null && Discriminators.Length > 0)
            {
                foreach (var discriminator in Discriminators)
                {
                    var str = string.Concat(";D_", discriminator.Column.Name, "=", discriminator.Value.ToString());
                    key += str;
                }
            }

            return key;
        }

        internal override void AssignMapping(ColumnMap columnMap, DbQueryCommandTree commandTree)
        {
            if (FromObjectQuery == null)
                base.AssignMapping(columnMap, commandTree);
        }

        public IList<EdmProperty> GetUnmappedRequiredProperties()
        {
            if (ColumnMap == null)
                return null;
            
            var result = _unmappedRequiredProperties;
            if (result == null)
            {
                Interlocked.CompareExchange(ref _unmappedRequiredProperties, BuildRequiredProperties(), null);
                result = _unmappedRequiredProperties;
            }
            
            return result == _emptyEdmProperty ? null : result;
        }

        private IList<EdmProperty> BuildRequiredProperties()
        {
            IList<EdmProperty> result = null;
            
            foreach (var edmProperty in TargetEntitySet.ElementType.Properties)
            {
                if (!edmProperty.Nullable && !ColumnMap.IsTargetMapped(edmProperty.Name) && !IsDiscriminatorMapped(edmProperty.Name))
                {
                    (result ?? (result = new List<EdmProperty>())).Add(edmProperty);
                }
            }

            return result ?? _emptyEdmProperty;
        }

        private bool IsDiscriminatorMapped(string columnName)
        {
            if (Discriminators == null || Discriminators.Length == 0)
                return false;
            foreach (var discriminator in Discriminators)
            {
                if (discriminator.Column.Name == columnName)
                    return true;
            }

            return false;
        }
    }

    public sealed class DmlColumnMapping
    {
        internal DmlColumnMapping(int nullSentinelOrdinal, int mappingsCount)
        {
            NullSentinelOrdinal = nullSentinelOrdinal;
            Mappings = new DmlColumnMap[mappingsCount];
        }

        internal bool IsTargetMapped(string targetColumnName)
        {
            for (var i = 0; i < Mappings.Length; i++)
            {
                if (Mappings[i].TargetColumnName == targetColumnName)
                    return true;
            }

            return false;
        }

        public int NullSentinelOrdinal { get; }
        public DmlColumnMap[] Mappings { get; }
    }

    public readonly struct DmlColumnMap
    {
        internal DmlColumnMap(int sourceOrdinal, string targetPropertyName, string targetColumnName)
        {
            SourceOrdinal = sourceOrdinal;
            TargetPropertyName = targetPropertyName;
            TargetColumnName = targetColumnName;
        }

        public int SourceOrdinal { get; }
        public string TargetPropertyName { get; }
        public string TargetColumnName { get; }
    }
}