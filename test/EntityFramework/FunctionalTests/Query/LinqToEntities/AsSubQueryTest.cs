// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Query.LinqToEntities
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using Xunit;

    public class AsSubQueryTest : FunctionalTestBase
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
            public DbSet<Book> Books { get; set; }
            public DbSet<Author> Authors { get; set; }

            
            public MyContext() : base(CreateConnection(), true)
            {
                Database.SetInitializer<MyContext>(null);
            }

            private static DbConnection CreateConnection()
            {
                return new SqlConnection("Data Source=pk8local;Initial Catalog=PK8_DATA_DEV;User ID=pk8_appserver;Password=pk8_app_pk8;Pooling=True;Min Pool Size=4;Max Pool Size=200;MultipleActiveResultSets=True;Connect Timeout=60;Application Name=Entity Framework tests");
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


        [Fact]
        public void CanQueryAsSubQuery()
        {
            using (var context = new MyContext())
            {
                var q1 = from b in context.Books
                    join a in context.Authors.Where(a => !a.Name.StartsWith("И")) on b.AuthorId equals a.Id
                    where a.Name != "ыыы"
                    select new { BookId = b.Id, b.Title, a.Name };

                var tr1 = q1.ToString();
                var l1 = q1.ToList();
                
                var q2 = from b in context.Books
                    join a in context.Authors.Where(a => !a.Name.StartsWith("И")).Take(int.MaxValue) on b.AuthorId equals a.Id
                    where a.Name != "ыыы"
                    select new { BookId = b.Id, b.Title, a.Name };

                var tr2 = q2.ToString();
                var l2 = q2.ToList();

                var q3 = from b in context.Books
                    join a in context.Authors.Where(a => !a.Name.StartsWith("И")).AsSubQuery() on b.AuthorId equals a.Id
                    // from a in context.Authors.Where(a => !a.Name.StartsWith("И") && a.Id == b.AuthorId).AsSubQuery()
                    where a.Name != "ыыы"
                    select new { BookId = b.Id, b.Title, a.Name };

                var tr3 = q3.ToString();
                var l3 = q3.ToList();

                var q4 = from a in context.Authors.Where(a => !a.Name.StartsWith("И")).AsSubQuery().Take(100200)
                    where a.Name != "ыыы"
                    select new { a.Id, a.Name };
                
                var tr4 = q4.ToString();
                var l4 = q4.ToList();

                var q5 = from a in context.Authors.Where(a => !a.Name.StartsWith("И")).AsSubQuery().Take(10).Where(e => e.Id != 40).AsSubQuery().AsSubQuery()
                    where a.Name != "ыыы"
                    select new { a.Id, a.Name };
                
                var tr5 = q5.ToString();
                var l5 = q5.ToList();

            }
        }
        
    }
}
