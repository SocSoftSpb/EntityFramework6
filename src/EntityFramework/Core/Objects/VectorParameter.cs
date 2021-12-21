// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Objects
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Provides ability to pass single-column Table-Valued-Parameter (Array in PostgreSQL) to a LINQ-To-Entities query 
    /// </summary>
    public abstract class VectorParameter : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }
    
    /// <summary>
    /// Provides ability to pass single-column Table-Valued-Parameter (Array in PostgreSQL) to a LINQ-To-Entities query 
    /// </summary>
    /// <typeparam name="T">
    /// A Non-Nullable primitive type such as byte, short, int, long, Guid, DateTime and string
    /// </typeparam>
    /// <remarks>
    /// Vector parameters can be used in various linq-to entities query.<br/>
    /// Queries with Vector parameters can be cached at EF level (Queries with List.Contains - can't).<br/>
    /// <br/>
    /// Examples:
    /// <p>
    /// ========== Contains ==========
    /// <code>
    /// VectorParameter&lt;byte&gt; vp = new() {1, 2, 3};
    /// 
    /// var query = from el in uow.Repository&lt;ErrorLog&gt;()
    ///             where vp.Contains(el.SeverityId)
    ///             select new { el.Id, el.Message };
    /// </code>
    /// -- MS SQL code:
    /// <code>
    /// SELECT [Extent1].[Id] AS [Id], [Extent1].[Message] AS [Message]
    /// FROM [Logging].[ErrorLog] AS [Extent1]
    /// WHERE  EXISTS (SELECT 1 AS [C1] FROM @p__linq__0 AS [Extent2] WHERE [Extent2].[Value] = [Extent1].[SeverityId])
    /// </code>
    /// -- PostgreSQL code:
    /// <code>
    /// SELECT "Extent1"."Id", "Extent1"."Message"
    /// FROM "Logging"."ErrorLog" AS "Extent1"
    /// WHERE "Extent1"."SeverityId" = ANY (@p__linq__0)
    /// </code>
    /// </p>
    /// <p><br/>
    /// ========== Join ==========
    /// <code>
    /// // Some list of Guids
    /// VectorParameter&lt;Guid&gt; vp = new(uow.Repository&lt;ErrorLog&gt;().Take(20).Select(e =&gt; e.Id).ToList());
    ///   
    /// var query = from el in uow.Repository&lt;ErrorLog&gt;()
    ///             join p in vp on el.Id equals p
    ///             select new { el.Id, el.Message };
    /// </code>
    /// -- MS SQL code:
    /// <code>
    /// SELECT [Extent1].[Id] AS [Id], [Extent1].[Message] AS [Message]
    /// FROM  [Logging].[ErrorLog] AS [Extent1]
    /// INNER JOIN @p__linq__0 AS [Extent2] ON [Extent1].[Id] = [Extent2].[Value]
    /// </code>
    /// -- PostgreSQL code:
    /// <code>
    /// SELECT "Extent1"."Id", "Extent1"."Message"
    /// FROM "Logging"."ErrorLog" AS "Extent1"
    /// INNER JOIN unnest(@p__linq__0) AS "Extent2"("Value") ON "Extent1"."Id" = "Extent2"."Value"
    /// </code>
    /// </p>
    /// </remarks>
    public class VectorParameter<T> : VectorParameter, ICollection<T>
    {
        private readonly ICollection<T> _col;

        /// <summary>
        /// Create immutable (Read-Only) VectorParameter from <paramref name="collection"/>
        /// </summary>
        public VectorParameter(IEnumerable<T> collection)
        {
            var lst = collection as IList<T> ?? collection.ToList();
            _col = lst.IsReadOnly ? lst : new ReadOnlyCollection<T>(lst);
        }

        /// <summary>
        /// Create mutable VectorParameter that allows modify the list of values
        /// </summary>
        public VectorParameter()
        {
            _col = new List<T>();
        }

        private IEnumerator<T> GetEnumeratorImpl() => _col.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumeratorImpl();

        /// <inheritdoc/>
        public override IEnumerator GetEnumerator() => GetEnumeratorImpl();

        /// <inheritdoc/>
        public void Add(T item) => _col.Add(item);

        /// <inheritdoc/>
        public void Clear() => _col.Clear();

        /// <inheritdoc/>
        public bool Contains(T item) => _col.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex) => _col.CopyTo(array, arrayIndex);
        
        /// <inheritdoc/>
        public bool Remove(T item) => _col.Remove(item);

        /// <inheritdoc/>
        public int Count => _col.Count;
        
        /// <summary>
        /// True if created from existing collection (<see cref="VectorParameter{T}(IEnumerable{T})"/>), False if created from parameterless constructor (<see cref="VectorParameter{T}()"/>)
        /// </summary>
        public bool IsReadOnly => _col.IsReadOnly;
    }
    
}
