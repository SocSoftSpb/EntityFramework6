// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.SqlServer.SqlGen
{
    internal class SelectOutputColumn : ISqlFragment
    {
        private readonly ISqlFragment _columnDefinition;
        private readonly Symbol _aliasSymbol;
        private readonly string _alias;

        public SelectOutputColumn(ISqlFragment columnDefinition, string @alias)
        {
            _columnDefinition = columnDefinition;
            _alias = alias;
        }

        public SelectOutputColumn(ISqlFragment columnDefinition, Symbol aliasSymbol)
        {
            _columnDefinition = columnDefinition;
            _aliasSymbol = aliasSymbol;
        }

        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            _columnDefinition.WriteSql(writer, sqlGenerator);
            writer.Write(" AS ");
            if (_aliasSymbol != null)
                _aliasSymbol.WriteSql(writer, sqlGenerator);
            else
                writer.Write(SqlGenerator.QuoteIdentifier(_alias));
        }

        public void WriteUpdate(SqlWriter writer, SqlGenerator sqlGenerator, string columnName)
        {
            writer.Write(SqlGenerator.QuoteIdentifier(columnName));
            writer.Write(" = ");
            _columnDefinition.WriteSql(writer, sqlGenerator);
        }
    }
}
