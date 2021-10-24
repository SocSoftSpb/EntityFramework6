// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Objects
{
    using System.Collections.Generic;
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Core.Common.CommandTrees.Internal;
    using System.Linq;
    using System.Linq.Expressions;

    public static class BatchDmlFactory
    {
        /// <summary>
        /// Create command for BatchDelete
        /// </summary>
        public static IBatchDeleteCommand CreateBatchDeleteQuery<T>(ObjectQuery<T> query, bool withRowCount)
        {
            ObjectQuery<int> q = withRowCount
                ? (ObjectQuery<int>)query.Select(e => DbDmlFunctions.DeleteMarkerRowCount(e))
                : (ObjectQuery<int>)query.Select(e => DbDmlFunctions.DeleteMarker(e));
                
            return new BatchDeleteCommand(q, withRowCount);
        }

        /// <summary>
        /// Create command for BatchUpdate
        /// </summary>
        public static IBatchUpdateCommand CreateBatchUpdateQuery<T>(ObjectQuery<T> query, Expression<Func<T, T>> setters, bool withRowCount, int limit = -1)
        {
            var q = (ObjectQuery<int>)DbDmlQueryFunctions.BatchUpdate(query, setters, withRowCount, limit);
            return new BatchUpdateCommand(q, withRowCount);
        }
        
        public static IBatchUpdateCommand CreateBatchUpdateJoinQuery<TQuery, TEntity>(ObjectQuery<TQuery> query, Expression<Func<TQuery, TEntity>> setters, bool withRowCount, int limit = -1)
        {
            var q = (ObjectQuery<int>)DbDmlQueryFunctions.BatchUpdateJoin(query, setters, withRowCount, limit);
            return new BatchUpdateCommand(q, withRowCount);
        }
        
        /// <summary>
        /// Create command for BatchInsert
        /// </summary>
        public static IBatchInsertCommand CreateBatchInsertQuery<T>(ObjectQuery<T> query, bool withRowCount)
        {
            var q = (ObjectQuery<int>)DbDmlQueryFunctions.BatchInsert(query, withRowCount);
            return new BatchInsertCommand(q, withRowCount);
        }

        private static int ExecuteBatchOperation(ObjectQuery<int> query, bool withRowCount)
        {
            if (withRowCount)
                return ((IEnumerable<int>)query).FirstOrDefault();

            query.QueryState.ObjectContext.AsyncMonitor.EnsureNotEntered();
            
            var executionStrategy = query.ExecutionStrategy
                                    ?? DbProviderServices.GetExecutionStrategy(
                                        query.QueryState.ObjectContext.Connection, query.QueryState.ObjectContext.MetadataWorkspace);
            
            executionStrategy.Execute(
                () => query.QueryState.ObjectContext.ExecuteInTransaction(
                    () => ExecuteDmlBatch(query),
                    executionStrategy,
                    startLocalTransaction: false,
                    releaseConnectionOnSuccess: true));

            return -1;
        }

        private static int ExecuteDmlBatch(ObjectQuery<int> query)
        {
            var execPlan = query.QueryState.GetExecutionPlan(MergeOption.NoTracking);
            execPlan.ExecuteNonQuery(query.QueryState.ObjectContext, query.QueryState.Parameters);
            return -1;
        }
        
        private abstract class BatchCommandBase : IBatchDmlCommand
        {
            private readonly ObjectQuery<int> _objectQuery;
            private readonly bool _withRowCount;

            protected BatchCommandBase(ObjectQuery<int> objectQuery, bool withRowCount)
            {
                _objectQuery = objectQuery;
                _withRowCount = withRowCount;
            }

            public int Execute()
            {
                return ExecuteBatchOperation(_objectQuery, _withRowCount);
            }

            public string ToTraceString() => _objectQuery.ToTraceString();
        }

        private sealed class BatchDeleteCommand : BatchCommandBase, IBatchDeleteCommand
        {
            public BatchDeleteCommand(ObjectQuery<int> objectQuery, bool withRowCount)
                : base(objectQuery, withRowCount)
            {
            }
        }
        
        private sealed class BatchUpdateCommand : BatchCommandBase, IBatchUpdateCommand
        {
            public BatchUpdateCommand(ObjectQuery<int> objectQuery, bool withRowCount)
                : base(objectQuery, withRowCount)
            {
            }
        }
        
        private sealed class BatchInsertCommand : BatchCommandBase, IBatchInsertCommand
        {
            public BatchInsertCommand(ObjectQuery<int> objectQuery, bool withRowCount)
                : base(objectQuery, withRowCount)
            {
            }
        }
        
        /// <summary>
        /// Class for Join Queryable for Update 
        /// </summary>
        public sealed class JoinTuple<TEntity, TSource>
        {
            /// <summary>
            /// Updatable query
            /// </summary>
            public TEntity Entity { get; set; }
            /// <summary>
            /// Source query
            /// </summary>
            public TSource Source { get; set; }
        }
    }

    /// <summary>
    /// Base interface for batch DML commands
    /// </summary>
    public interface IBatchDmlCommand
    {
        /// <summary>Returns the commands to execute against the data source.</summary>
        /// <returns>A string that represents the commands that the query executes against the data source.</returns>
        string ToTraceString();

        /// <summary>
        /// Execute DML.
        /// </summary>
        /// <returns>Row count if command created with Rows Count. Otherwise <c>-1</c>.</returns>
        int Execute();
    }
    
    /// <summary>
    /// Command for batch delete
    /// </summary>
    public interface IBatchDeleteCommand : IBatchDmlCommand
    {
    }
    
    /// <summary>
    /// Command for batch update
    /// </summary>
    public interface IBatchUpdateCommand : IBatchDmlCommand
    {
    }
    
    /// <summary>
    /// Command for batch insert
    /// </summary>
    public interface IBatchInsertCommand : IBatchDmlCommand
    {
    }
    
}
