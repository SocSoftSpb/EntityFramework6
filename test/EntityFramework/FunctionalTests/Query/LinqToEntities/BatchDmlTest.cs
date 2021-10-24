// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Query.LinqToEntities
{
    using System.Data.Common;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using Xunit;

    public class BatchDmlTest : FunctionalTestBase
    {
        public class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int AuthorId { get; set; }
            public int? SomeNullableInt { get; set; }
            public Author Author { get; set; }
        }
        
        public class Book1 : Book
        {
            
        }

        public class Book2 : Book
        {
            
        }

        public class Author
        {
            public int Id { get; set; }
            public string Name { get; set; }
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
                b.Entity<Author>();
                b.Entity<Book>()
                    .HasRequired(e => e.Author).WithMany().HasForeignKey(e => e.AuthorId);

                b.Entity<Book>().Map(e => e.Requires("Disc").HasValue((byte)3));
                b.Entity<Book1>().Map(e => e.Requires("Disc").HasValue((byte)11));
                b.Entity<Book2>().Map(e => e.Requires("Disc").HasValue((byte)22));
            }

            // public ObjectQuery<Book> Books => CreateObjectSet<Book>();
        }

        [Fact]
        public void CanBatchDeleteBooks()
        {
            var contextInfo = new DbContextInfo(typeof(MyContext), ProviderRegistry.Sql2008_ProviderInfo);

            using (var context = contextInfo.CreateInstance())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                IQueryable<Book> books = objectContext.CreateObjectSet<Book>();
                
                var toDelete = books.Where(e => e.Title.Contains("aaa") && e.Author.Name.StartsWith("И"))
                    //.OrderBy(e => e.Id).ThenBy(e => e.Title)
                    .Take(100)
                    ;

                var toDeleteObjectQuery = (ObjectQuery<Book>)toDelete;

                var deleteCommand = BatchDmlFactory.CreateBatchDeleteQuery(toDeleteObjectQuery, true);

                var str = deleteCommand.ToTraceString();
            }
        }

        [Fact]
        public void CanBatchUpdateBooks()
        {
            using (var context = new MyContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                IQueryable<Book> books = objectContext.CreateObjectSet<Book>();

                var toUpdate = books.Where(
                    e => e.Title.Contains("aaa")
                         && e.Author.Name.StartsWith("И")
                );

                var fromQuery = (ObjectQuery<Book>)toUpdate;
                var updateQuery = BatchDmlFactory.CreateBatchUpdateQuery(
                    fromQuery,
                    e => new Book
                    {
                        Title = e.Title + " " + e.Author.Name,
                        SomeNullableInt = null
                        // AuthorId = 100
                    }, true, 120);
                var strToUpdate = updateQuery.ToTraceString();

                var result = updateQuery.Execute();
            }
        }

        public class AuthorInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void CanBatchUpdateBooksWithJoin()
        {
            using (var context = new MyContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                IQueryable<Book> books = objectContext.CreateObjectSet<Book>();
                IQueryable<Author> authors = objectContext.CreateObjectSet<Author>();

                var aa = from a1 in authors
                    join a2 in authors on a1.Name equals a2.Name
                    select new AuthorInfo { Id = a1.Id, Name = a1.Name };

                var toUpdate = books.Join(aa, b => b.AuthorId, a => a.Id, 
                    (b, a) => new BatchDmlFactory.JoinTuple<Book, AuthorInfo>{Entity = b, Source = a})
                    .Where(
                    e => e.Entity.Title.Contains("aaa")
                         && e.Source.Name.StartsWith("И")
                );

                var fromQuery = (ObjectQuery<BatchDmlFactory.JoinTuple<Book, AuthorInfo>>)toUpdate;
                var updateQuery = BatchDmlFactory.CreateBatchUpdateJoinQuery(
                    fromQuery,
                    e => new Book
                    {
                        Title = e.Entity.Title + " " + e.Source.Name,
                        SomeNullableInt = null
                        // AuthorId = 100
                    }, true, 120);
                var strToUpdate = updateQuery.ToTraceString();

                var result = updateQuery.Execute();
            }
        }

        [Fact]
        public void CanBatchInsertBooks()
        {
            using (var context = new MyContext())
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    IQueryable<Author> authors = objectContext.CreateObjectSet<Author>();

                    // var b = new Book1
                    // {
                    //     Id = 3412,
                    //     AuthorId = 1,
                    //     Title = "Mushrooms",
                    // };
                    //
                    // context.Set<Book>().Add(b);
                    //
                    // context.SaveChanges();

                    var toInsert = authors.Where(
                            e => !e.Name.Contains("aaa")
                        )
                        .Take(20)
                        .Select(
                            e => new Book
                            {
                                Id = e.Id + 100500,
                                Title = "Book written by " + e.Name,
                                AuthorId = e.Id
                            });

                    var fromQuery = (ObjectQuery<Book>)toInsert;
                    var insertQuery = BatchDmlFactory.CreateBatchInsertQuery(fromQuery, true);
                    var strInsert = insertQuery.ToTraceString();

                    var result = insertQuery.Execute();

                    tr.Rollback();
                }
        }


    }
}
