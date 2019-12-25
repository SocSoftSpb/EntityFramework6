// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.SqlServer.SqlGen
{
    using System.Globalization;

    internal class OptionClause : ISqlFragment
    {
        private readonly QueryOptions _queryOptions;

        public OptionClause(QueryOptions queryOptions)
        {
            _queryOptions = queryOptions;
        }

        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            var optionCount = 0;
            writer.WriteLine();
            writer.Write("OPTION(");
            if (_queryOptions.Recompile)
                WriteOption(writer, ref optionCount, "RECOMPILE");
            if (_queryOptions.MaxDop != null)
                WriteOption(writer, ref optionCount, "MAXDOP", _queryOptions.MaxDop.Value.ToString(CultureInfo.InvariantCulture));
            writer.Write(")");
        }

        private static void WriteOption(SqlWriter writer, ref int optionCount, string name, string value = null)
        {
            if (optionCount > 0)
                writer.Write(", ");
            writer.Write(name);
            if (value != null)
            {
                writer.Write(" ");
                writer.Write(value);
            }

            optionCount++;
        }
    }
}
