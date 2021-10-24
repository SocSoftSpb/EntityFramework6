// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.SqlServer.SqlGen
{
    using System.Collections.Generic;
    using System.Diagnostics;

    // <summary>
    // This class is used for building the SELECT clause of a Sql Statement
    // It is used to gather information about required and optional columns
    // and whether TOP and DISTINCT should be specified.
    // The underlying SqlBuilder is used for gathering the required columns.
    // The list of OptionalColumns is used for gathering the optional columns.
    // Whether a given OptionalColumn should be written is known only after the entire
    // command tree has been processed.
    // The IsDistinct property indicates that we want distinct columns.
    // This is given out of band, since the input expression to the select clause
    // may already have some columns projected out, and we use append-only SqlBuilders.
    // The DISTINCT is inserted when we finally write the object into a string.
    // Also, we have a Top property, which is non-null if the number of results should
    // be limited to certain number. It is given out of band for the same reasons as DISTINCT.
    // </summary>
    internal class SqlSelectClauseBuilder : ISqlFragment
    {
        #region Fields and Properties

        public List<OptionalColumn> OptionalColumns { get; private set; }

        public List<SelectOutputColumn> Columns { get; private set; }

        internal void AddOptionalColumn(OptionalColumn column)
        {
            if (OptionalColumns == null)
            {
                OptionalColumns = new List<OptionalColumn>();
            }
            OptionalColumns.Add(column);
        }

        private TopClause m_top;

        internal TopClause Top
        {
            get { return m_top; }
            set
            {
                Debug.Assert(m_top == null, "SqlSelectStatement.Top has already been set");
                m_top = value;
            }
        }

        private SkipClause m_skip;

        internal SkipClause Skip
        {
            get { return m_skip; }
            set
            {
                Debug.Assert(m_skip == null, "SqlSelectStatement.Skip has already been set");
                m_skip = value;
            }
        }

        // <summary>
        // Do we need to add a DISTINCT at the beginning of the SELECT
        // </summary>
        internal bool IsDistinct { get; set; }

        // <summary>
        // Whether any columns have been specified.
        // </summary>
        public bool IsEmpty
        {
            get { return  (Columns == null || Columns.Count == 0) && (OptionalColumns == null || OptionalColumns.Count == 0); }
        }

        private readonly Func<bool> m_isPartOfTopMostStatement;

        #endregion

        #region Constructor

        internal SqlSelectClauseBuilder(Func<bool> isPartOfTopMostStatement)
        {
            m_isPartOfTopMostStatement = isPartOfTopMostStatement;
        }

        public SqlSelectDmlModificator DmlModificator { get; set; }


        #endregion

        #region Public Methods

        public void AddColumn(ISqlFragment columnDefinition, string alias)
        {
            if (Columns == null)
                Columns = new List<SelectOutputColumn>();
            Columns.Add(new SelectOutputColumn(columnDefinition, alias));
        }

        public void AddColumn(ISqlFragment columnDefinition, Symbol columnSymbol)
        {
            if (Columns == null)
                Columns = new List<SelectOutputColumn>();
            Columns.Add(new SelectOutputColumn(columnDefinition, columnSymbol));
        }

        #endregion

        #region ISqlFragment Members

        // <summary>
        // Writes the string representing the Select statement:
        // SELECT (DISTINCT) (TOP topClause) (optionalColumns) (requiredColumns)
        // If Distinct is specified or this is part of a top most statement
        // all optional columns are marked as used.
        // Optional columns are only written if marked as used.
        // In addition, if no required columns are specified and no optional columns are
        // marked as used, the first optional column is written.
        // </summary>
        public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (DmlModificator == null || !DmlModificator.ProcessSqlClauseOperatorName(writer, sqlGenerator))
                writer.Write("SELECT ");

            if (IsDistinct)
            {
                writer.Write("DISTINCT ");
            }

            if (Top != null && Skip == null)
            {
                Top.WriteSql(writer, sqlGenerator);
            }

            if (DmlModificator != null && !DmlModificator.WrapAllInSubquery)
            {
                // do nothing
            }
            else if (IsEmpty)
            {
                Debug.Assert(false); // we have removed all possibilities of SELECT *.
                writer.Write("*");
            }
            else
            {
                var printedAny = false;

                if (DmlModificator == null
                    || !DmlModificator.WriteSelectColumns(writer, sqlGenerator, this))
                {
                    //Print the optional columns if any
                    if (OptionalColumns != null
                        && (DmlModificator == null || DmlModificator.AllowOptionalColumns()))
                        printedAny = WriteOptionalColumns(writer, sqlGenerator);

                    if (Columns != null)
                    {
                        foreach (var column in Columns)
                        {
                            if (printedAny)
                                writer.Write(", ");

                            writer.WriteLine();
                            column.WriteSql(writer, sqlGenerator);
                            printedAny = true;
                        }
                    }
                    //If no optional columns were printed and there were no other columns, 
                    // print at least the first optional column
                    else if (!printedAny)
                    {
                        OptionalColumns[0].MarkAsUsed();
                        OptionalColumns[0].WriteSqlIfUsed(writer, sqlGenerator, "");
                    }
                }
            }
        }

        #endregion

        #region Private Helper Methods

        // <summary>
        // Writes the optional columns that are used.
        // If this is the topmost statement or distinct is specified as part of the same statement
        // all optional columns are written.
        // </summary>
        // <returns> Whether at least one column got written </returns>
        private bool WriteOptionalColumns(SqlWriter writer, SqlGenerator sqlGenerator)
        {
            if (OptionalColumns == null)
            {
                return false;
            }

            if (m_isPartOfTopMostStatement() || IsDistinct)
            {
                foreach (var column in OptionalColumns)
                {
                    column.MarkAsUsed();
                }
            }

            var separator = "";
            var printedAny = false;
            foreach (var column in OptionalColumns)
            {
                if (column.WriteSqlIfUsed(writer, sqlGenerator, separator))
                {
                    printedAny = true;
                    separator = ", ";
                }
            }
            return printedAny;
        }

        #endregion
    }
}
