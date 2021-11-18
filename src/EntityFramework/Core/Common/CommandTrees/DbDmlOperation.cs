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
        public DbDmlDeleteOperation(EntitySet targetEntitySet, bool withRowCount, int limit)
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

        protected DbDmlUpInsOperationBase(DbDmlOperationKind kind, EntitySet targetEntitySet, Type clrEntityType, bool withRowCount)
            : base(kind, targetEntitySet, withRowCount)
        {
            ClrEntityType = clrEntityType;
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

                dmlMap.Mappings[index] = new DmlColumnMap(scalarColumnMap.ColumnPos, scalarColumnMap.Name);
            }

            ColumnMap = dmlMap;
        }
    }

    public sealed class DbDmlUpdateOperation : DbDmlUpInsOperationBase
    {
        public DbDmlUpdateOperation(EntitySet targetEntitySet, Type clrEntityType, bool withRowCount, int limit)
            : base(DbDmlOperationKind.Update, targetEntitySet, clrEntityType, withRowCount)
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
        public ValueConditionMapping[] Discriminators { get; }
        public ObjectQuery FromObjectQuery { get; }
        public ObjectQueryColumnMap[] FromQueryMapping { get; }
        public DbCommand FromStoreCommand { get; }
        internal IEnumerable<Tuple<ObjectParameter, QueryParameterExpression>> LinqParameters { get; }

        public DbDmlInsertOperation(EntitySet targetEntitySet, Type clrEntityType, bool withRowCount, ValueConditionMapping[] discriminators, ObjectQuery fromObjectQuery)
            : base(DbDmlOperationKind.Insert, targetEntitySet, clrEntityType, withRowCount)
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
    }

    public sealed class DmlColumnMapping
    {
        public DmlColumnMapping(int nullSentinelOrdinal, int mappingsCount)
        {
            NullSentinelOrdinal = nullSentinelOrdinal;
            Mappings = new DmlColumnMap[mappingsCount];
        }

        public int NullSentinelOrdinal { get; }
        public DmlColumnMap[] Mappings { get; }
    }

    public readonly struct DmlColumnMap
    {
        public DmlColumnMap(int sourceOrdinal, string targetName)
        {
            SourceOrdinal = sourceOrdinal;
            TargetName = targetName;
        }

        public int SourceOrdinal { get; }
        public string TargetName { get; }
    }
}