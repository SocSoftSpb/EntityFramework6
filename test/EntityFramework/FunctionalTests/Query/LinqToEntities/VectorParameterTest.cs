// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Query.LinqToEntities
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using Xunit;

    public class VectorParameterTest : FunctionalTestBase
    {
        public class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int AuthorId { get; set; }
            public int? SomeNullableInt { get; set; }
            public Author Author { get; set; }
        }
        
        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public AuthorType AuthorType { get; set; }
        }

        public enum AuthorType
        {
            None = 0,
            Writer = 1,
            Poet = 2
        }

        public class MyContext : DbContext
        {
            public MyContext() : base(CreateConnection(), true)
            {
                Database.SetInitializer<MyContext>(null);
            }

            private static DbConnection CreateConnection()
            {
                return new SqlConnection("Data Source=pk8dev;Initial Catalog=PK8_DATA;User ID=pk8_appserver;Password=pk8_app_pk8;Pooling=True;Min Pool Size=4;Max Pool Size=200;MultipleActiveResultSets=True;Connect Timeout=60;Application Name=Entity Framework tests");
            }

            protected override void OnModelCreating(DbModelBuilder b)
            {
                b.Entity<Book>().HasRequired(e => e.Author).WithMany().HasForeignKey(e => e.AuthorId);
                
                b.VectorParameter<int>()
                    .HasStoreType("Objects", "IntParam");
                b.VectorParameter<string>()
                    .HasStoreType("Objects", "StringParam");
            }
        }
        
        public class MyObjectContext : ObjectContext
        {
            public MyObjectContext() : base(CreateConnection(), true)
            {
                
            }

            private static EntityConnection CreateConnection()
            {
                using (var context = new MyContext())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    return new EntityConnection(objectContext.MetadataWorkspace, CreateStoreConnection(), true);
                }
            }
            
            private static DbConnection CreateStoreConnection()
            {
                return new SqlConnection("Data Source=pk8dev;Initial Catalog=PK8_DATA;User ID=pk8_appserver;Password=pk8_app_pk8;Pooling=True;Min Pool Size=4;Max Pool Size=200;MultipleActiveResultSets=True;Connect Timeout=60;Application Name=Entity Framework tests");
            }

            public ObjectSet<Book> Books => CreateObjectSet<Book>();
        }

        [Fact]
        public void CanQueryBookByVectorContains()
        {
            using (var context = new MyContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;

                var books = objectContext.CreateObjectSet<Book>();

                // var bb = books.Select(e => e.AuthorId);

                var param = new VectorParameter<int>(new[] { 1, 2, 3, 4 });
                var queryable = books.Where(e => param.Contains(e.Id));
                var trace = ((ObjectQuery)queryable).ToTraceString();
                var lst = queryable.ToList();
                // var lst = books.Where(e => e.Id == iid).ToList();
            }
        }
        
        [Fact]
        public void CanQueryBookWithVariousVectorQuery()
        {
            using (var context = new MyContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;

                var books = objectContext.CreateObjectSet<Book>();

                var param = new VectorParameter<int>(new[] { 1, 2, 3, 4 });
        
                var queryable = books.Where(e => param.Where(x => x >= 1 && x <= 3).Contains(e.Id));
                var trace = ((ObjectQuery)queryable).ToTraceString();
                var lst = queryable.ToList();
                
                // Test plan caching
                // Ensure that in ELinqQueryState.GetExecutionPlan()
                // if (cacheManager.TryCacheLookup(cacheKey, out executionPlan)) -> true
                var queryable2 = books.Where(e => param.Where(x => x >= 1 && x <= 3).Contains(e.Id));
                var lst2 = queryable2.ToList();
                
                // Test join
                var qJoin = from b in books
                    join p in param on b.AuthorId equals p
                    select new { b.Id, b.Title, AuthorName = b.Author.Name };
                var lstJoin = qJoin.ToList();
                
                var paramNames = new VectorParameter<string>(new[]{"aaa", "xxx", "bbb"});
                
                // Test join
                var qJoin2 = from b in books
                    join p in param on b.AuthorId equals p
                    where paramNames.Any(x => b.Title.Contains(x))
                    select new { b.Id, b.Title, AuthorName = b.Author.Name };
                var lstJoin2 = qJoin2.ToList();

            }
        }
        
        [Fact]
        public void VectorParametersMustBeSingleInstantiated()
        {
            using (var context = new MyContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                
                var books = objectContext.CreateObjectSet<Book>();

                var param = new VectorParameter<int>(new[] { 1, 2, 3, 4 });
                var paramNames = new VectorParameter<string>(new[]{"aaa", "xxx", "bbb"});
                
                // Test join
                var qJoin2 = from b in books
                    join p in param on b.AuthorId equals p
                    where paramNames.Any(x => b.Title.Contains(x)) 
                          || paramNames.Any(e => b.Author.Name == e)
                          || param.Contains(b.AuthorId)
                    select new { b.Id, b.Title, AuthorName = b.Author.Name };

                var trace = ((ObjectQuery)qJoin2).ToTraceString();
                var lstJoin2 = qJoin2.ToList();
            }
        }
        
        private sealed class BookProj
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }

        [Fact]
        public void VectorParametersCanBeUsedInCompiledQueries()
        {
            using (var context = new MyObjectContext())
            {
                var comp = CompiledQuery.Compile<MyObjectContext, int, int, VectorParameter<int>, IEnumerable<BookProj>>(
                    (ctx, x, y, bookIds) => from b in ctx.Books
                        where b.AuthorId.Between(x, y) && bookIds.Contains(b.Id)
                        select new BookProj { Id = b.Id, Title = b.Title }
                );

                var vp = new VectorParameter<int>(new[] { 1, 2, 3, 4 });
                var lst = comp(context, 2, 3, vp).ToList();
                
                vp = new VectorParameter<int>(new[] { 1, 2, 3, 4, 5, 6 });
                lst = comp(context, 1, 5, vp).ToList();
            }
        }

        [Fact]
        public void CanPassEmptyVectorParameter()
        {
            using (var context = new MyObjectContext())
            {
                var books = context.CreateObjectSet<Book>();
                var vp = new VectorParameter<int>(new int[0]);
                var queryable = books.Where(e => vp.Contains(e.Id));
                var trace = ((ObjectQuery)queryable).ToTraceString();
                var lst = queryable.ToList();
            }
        }
    }
}
