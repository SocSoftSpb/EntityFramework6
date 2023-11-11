// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.SqlServer.SqlGen
{
    using System.Data.Entity.Core.Common.CommandTrees;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Linq;

    internal abstract class SqlSelectDmlModificator
    {
        public abstract bool ProcessSqlClauseOperatorName(SqlWriter writer, SqlGenerator sqlGenerator);

        public abstract void WriteSubqueryHeader(SqlWriter writer, SqlGenerator sqlGenerator);

        public abstract void WriteSubqueryFooter(SqlWriter writer, SqlGenerator sqlGenerator);

        public abstract void WriteFromClause(SqlWriter writer, SqlGenerator sqlGenerator);

        public abstract bool WrapAllInSubquery { get; }

        public abstract bool WithRowCount { get; }

        public virtual bool WriteSelectColumns(SqlWriter writer, SqlGenerator sqlGenerator, SqlSelectClauseBuilder selectBuilder)
        {
            return false;
        }

        public virtual bool AllowOptionalColumns() => true;

        public virtual void AnalyzeExtent(Symbol fromAlias)
        {
        }
    }

    internal sealed class SqlSelectDeleteModificator : SqlSelectDmlModificator
    {
        public bool WrapDeletionInSubquery { get; }
        public Symbol FromExtent { get; }

        public override bool WrapAllInSubquery => WrapDeletionInSubquery;

        public override bool WithRowCount { get; }
        
        public int Limit { get; }


        public SqlSelectDeleteModificator(bool wrapDeletionInSubquery, Symbol fromExtent, DbDmlDeleteOperation dmlOperation)
        {
            WrapDeletionInSubquery = wrapDeletionInSubquery;
            FromExtent = fromExtent;
            WithRowCount = dmlOperation.WithRowCount;
            Limit = dmlOperation.Limit;
        }

        public override bool ProcessSqlClauseOperatorName(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (!WrapDeletionInSubquery)
            {
                writer.Write("DELETE ");
                if (Limit >= 0)
                    writer.Write($"TOP ({Limit}) ");
                return true;
            }

            return false;
        }

        public override void WriteSubqueryHeader(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (WrapDeletionInSubquery)
            {
                writer.Write("DELETE ");
                if (Limit >= 0)
                    writer.Write($"TOP ({Limit}) ");
                writer.Write("FROM __dml_query FROM (");
                writer.WriteLine();
            }
        }

        public override void WriteSubqueryFooter(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (WrapDeletionInSubquery)
            {
                writer.WriteLine();
                writer.Write(") AS __dml_query");
            }
        }

        public override void WriteFromClause(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (!WrapDeletionInSubquery)
            {
                writer.Write("FROM ");
                writer.Write(SqlGenerator.QuoteIdentifier(FromExtent.Name));
                writer.WriteLine();
            }
        }
    }

    internal sealed class SqlSelectUpdateModificator : SqlSelectDmlModificator
    {
        public SqlSelectStatement SelectStatement { get; }
        public DbDmlUpdateOperation DmlOperation { get; }
        
        public SqlSelectUpdateModificator(DbDmlUpdateOperation dmlOperation, SqlSelectStatement selectStatement)
        {
            SelectStatement = selectStatement;
            DmlOperation = dmlOperation;
        }

        public override bool ProcessSqlClauseOperatorName(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            writer.Write("UPDATE ");

            if (SelectStatement.Select.Top != null || SelectStatement.Select.Skip != null)
                throw new InvalidOperationException("Direct Take / Skip is not supported in Updatable queries.");

            if (DmlOperation.Limit >= 0)
            {
                writer.Write("TOP (");
                writer.Write(DmlOperation.Limit.ToString());
                writer.Write(") ");
            }
            
            return true;
        }

        public override void WriteSubqueryHeader(SqlWriter writer, SqlGenerator sqlGenerator)
        {
        }

        public override void WriteSubqueryFooter(SqlWriter writer, SqlGenerator sqlGenerator)
        {
        }

        public override void WriteFromClause(SqlWriter writer, SqlGenerator sqlGenerator)
        {
        }

        public override bool AllowOptionalColumns() => false;
        public override bool WrapAllInSubquery => true;
        public override bool WithRowCount => DmlOperation.WithRowCount;
        public Symbol TargetExtent { get; private set; }

        public override bool WriteSelectColumns(SqlWriter writer, SqlGenerator sqlGenerator, SqlSelectClauseBuilder selectBuilder)
        {
            if (selectBuilder != SelectStatement.Select)
                throw new InvalidOperationException("sqlSelectClauseBuilder != SelectStatement.Select");

            // var extent = SelectStatement.FromExtents[0];
            if (TargetExtent == null)
                throw new InvalidOperationException("Target extent is not found in translated query.");
                
            TargetExtent.WriteSql(writer, sqlGenerator);
            writer.WriteLine();
            writer.Write("SET");
            writer.Indent++;

            if (selectBuilder.OptionalColumns != null)
                throw new InvalidOperationException("OptionalColumns is not supported in Update.");

            if (selectBuilder.Columns == null || selectBuilder.Columns.Count == 0)
                throw new InvalidOperationException("Must be any column projection in Update.");

            var hasAnyColumn = false; 
            for (var i = 0; i < selectBuilder.Columns.Count; i++)
            {
                var column = selectBuilder.Columns[i];
                var map = DmlOperation.ColumnMap.Mappings.FirstOrDefault(e => e.SourceOrdinal == i);
                if (map.TargetPropertyName == null)
                {
                    if (i == DmlOperation.ColumnMap.NullSentinelOrdinal)
                        continue;
                    
                    throw new InvalidOperationException($"Can't find map for column # {i}.");
                }

                if (hasAnyColumn)
                    writer.Write(", ");

                writer.WriteLine();
                column.WriteUpdate(writer, sqlGenerator, map.TargetColumnName);

                hasAnyColumn = true;
            }

            writer.Indent--;
            
            return true;
        }

        public override void AnalyzeExtent(Symbol fromAlias)
        {
            if (TargetExtent != null)
                return;

            if (fromAlias.Type != null && fromAlias.Type.EdmType == DmlOperation.TargetEntitySet.ElementType)
                TargetExtent = fromAlias;
        }
    }
    
    internal sealed class SqlSelectInsertModificator : SqlSelectDmlModificator
    {
        public SqlSelectStatement SelectStatement { get; }
        public DbDmlInsertOperation DmlOperation { get; }
        
        public SqlSelectInsertModificator(DbDmlInsertOperation dmlOperation, SqlSelectStatement selectStatement)
        {
            SelectStatement = selectStatement;
            DmlOperation = dmlOperation;
        }

        public override bool ProcessSqlClauseOperatorName(SqlWriter writer, SqlGenerator sqlGenerator) => false;

        public override bool WriteSelectColumns(SqlWriter writer, SqlGenerator sqlGenerator, SqlSelectClauseBuilder selectBuilder)
        {
            if (selectBuilder.OptionalColumns != null)
                throw new InvalidOperationException("OptionalColumns is not supported in Update.");

            if (selectBuilder.Columns == null || selectBuilder.Columns.Count == 0)
                throw new InvalidOperationException("Must be any column projection in Update.");

            var mappings = DmlOperation.ColumnMap.Mappings;
            var outColumns = new SelectOutputColumn[mappings.Length];
            
            for (var i = 0; i < selectBuilder.Columns.Count; i++)
            {
                var column = selectBuilder.Columns[i];
                var found = false;
                
                for (var iMap = 0; iMap < mappings.Length; iMap++)
                {
                    if (mappings[iMap].SourceOrdinal == i)
                    {
                        if (outColumns[iMap] != null)
                            throw new InvalidOperationException($"Column {mappings[iMap].TargetPropertyName} is mapped twice.");
                        outColumns[iMap] = column;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (i == DmlOperation.ColumnMap.NullSentinelOrdinal)
                        continue;
                    
                    throw new InvalidOperationException($"Can't find map for column # {i}.");
                }
            }

            for (var i = 0; i < outColumns.Length; i++)
            {
                var outColumn = outColumns[i];
                if (outColumn == null)
                    throw new InvalidOperationException($"Can't find map for column {mappings[i].TargetPropertyName}.");
                
                if (i > 0)
                    writer.Write(", ");

                writer.WriteLine();
                outColumn.WriteSql(writer, sqlGenerator);
            }
            
            var unmappedRequiredProperties = DmlOperation.GetUnmappedRequiredProperties();
            if (unmappedRequiredProperties != null)
            {
                foreach (var edmProperty in unmappedRequiredProperties)
                {
                    writer.Write(", ");
                    writer.WriteLine();
                    
                    var sqlType = DmlOperation.TargetDataSpace == DataSpace.CSpace
                        ? sqlGenerator.GetSqlPrimitiveType(edmProperty.TypeUsage)
                        : sqlGenerator.GenerateSqlForStoreType(edmProperty.TypeUsage);
                    writer.Write("CAST(");
                    writer.Write(SqlGenerator.GetDefaultPrimitiveLiteral(edmProperty.TypeUsage));
                    writer.Write(" AS ");
                    writer.Write(sqlType);
                    writer.Write(")");
                }
            }

            if (DmlOperation.Discriminators != null && DmlOperation.Discriminators.Length > 0)
            {
                foreach (var discriminator in DmlOperation.Discriminators)
                {
                    writer.Write(",");
                    writer.WriteLine();
                    
                    var discValue = discriminator.Column.TypeUsage.Constant(discriminator.Value);
                    var discValueVisited = sqlGenerator.Visit(discValue);
                    var column = new SelectOutputColumn(discValueVisited , "__discriminator__" + discriminator.Column.Name);
                    
                    column.WriteSql(writer, sqlGenerator);
                }
            }

            return true;
        }

        public override void WriteSubqueryHeader(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            writer.Write("INSERT INTO ");
            if (!string.IsNullOrEmpty(DmlOperation.TargetEntitySet.Database))
            {
                writer.Write(SqlGenerator.QuoteIdentifier(DmlOperation.TargetEntitySet.Database));
                writer.Write(".");
            }
            
            if (!string.IsNullOrEmpty(DmlOperation.TargetEntitySet.Schema))
            {
                writer.Write(SqlGenerator.QuoteIdentifier(DmlOperation.TargetEntitySet.Schema));
                writer.Write(".");
            }

            writer.Write(SqlGenerator.QuoteIdentifier(DmlOperation.TargetEntitySet.Table));
            writer.Write(" (");

            if (DmlOperation.ColumnMap == null || DmlOperation.ColumnMap.Mappings.Length == 0)
                throw new InvalidOperationException("Column mapping is not set for Insert operation.");
            
            var mappings = DmlOperation.ColumnMap.Mappings;
            for (var i = 0; i < mappings.Length; i++)
            {
                if (i > 0)
                    writer.Write(", ");

                var map = mappings[i];
                writer.Write(SqlGenerator.QuoteIdentifier(map.TargetColumnName));
            }

            var unmappedRequiredProperties = DmlOperation.GetUnmappedRequiredProperties();
            if (unmappedRequiredProperties != null)
            {
                foreach (var edmProperty in unmappedRequiredProperties)
                {
                    writer.Write(", ");
                    writer.Write(SqlGenerator.QuoteIdentifier(edmProperty.Name));
                }
            }

            if (DmlOperation.Discriminators != null && DmlOperation.Discriminators.Length > 0)
            {
                foreach (var discriminator in DmlOperation.Discriminators)
                {
                    writer.Write(", ");
                    writer.Write(SqlGenerator.QuoteIdentifier(discriminator.Column.Name));
                }
            }

            writer.Write(")");
            writer.WriteLine();
        }

        public override void WriteSubqueryFooter(SqlWriter writer, SqlGenerator sqlGenerator)
        {
        }

        public override void WriteFromClause(SqlWriter writer, SqlGenerator sqlGenerator)
        {
        }

        public override bool AllowOptionalColumns() => false;
        public override bool WrapAllInSubquery => true;
        public override bool WithRowCount => DmlOperation.WithRowCount;
    }
}
