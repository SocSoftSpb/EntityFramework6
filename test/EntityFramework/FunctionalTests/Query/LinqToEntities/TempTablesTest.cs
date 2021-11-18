// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Query.LinqToEntities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Common;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Xunit;

    public class TempTablesTest : FunctionalTestBase
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
            }

            private IQueryProvider GetQueryProvider()
            {
                var objectContext = ((IObjectContextAdapter)this).ObjectContext;
                var piQueryProvider = typeof(ObjectContext).GetProperty("QueryProvider", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? throw new NullReferenceException("QueryProvider");
                return (IQueryProvider)piQueryProvider.GetValue(objectContext);
            }

            public IQueryable<T> DynamicQuery<T>(string query)
            {
                var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(T));
                return DynamicQuery<T>(query, options);
            }

            public IQueryable<T> DynamicQuery<T>(string query, DynamicEntitySetOptions options)
            {
                var queryProvider = GetQueryProvider();
                var queryExpression = DynamicQueryUtils.CreateDynamicQueryExpression(typeof(T), query, options);

                return queryProvider.CreateQuery<T>(queryExpression);
            }
        }

        internal static class DynamicQueryUtils
        {
            private static readonly MethodInfo _miDynamicSql;

            static DynamicQueryUtils()
            {
                Func<string, DynamicEntitySetOptions, IQueryable<object>> f = DynamicSqlUtilities.DynamicSql<object>;
                _miDynamicSql = f.Method.GetGenericMethodDefinition();
            }
            
            public static DynamicEntitySetOptions CreateDynamicQueryOptions(Type elementType)
            {
                var options = new DynamicEntitySetOptions();
                foreach (var pi in elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attr = pi.GetCustomAttribute<NotMappedAttribute>();
                    if (attr != null)
                        continue;
                    options.Columns.Add(new ColumnOption(pi));
                }

                return options;
            }
            
            public static Expression CreateDynamicQueryExpression(Type elementType, string query, DynamicEntitySetOptions options)
            {
                return Expression.Call(_miDynamicSql.MakeGenericMethod(elementType), Expression.Constant(query, typeof(string)), Expression.Constant(options, typeof(DynamicEntitySetOptions)));
            }
        }

        public class DynamicVersion
        {
            public string DatabaseVersion { get; set; }
            public bool IsUpdating { get; set; }
            public int UpdatePackCount { get; set; }
        }
        
        public class TempBook
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int AuthorId { get; set; }
        }
        
        [Fact]
        public void CanQueryDynamicSql()
        {
            using (var context = new MyContext())
            {
                var definingQuery = "SELECT" + " [DatabaseVersion], [IsUpdating], [UpdatePackCount] FROM [Configuration].[Version]";
                var dynQ = context.DynamicQuery<DynamicVersion>(definingQuery);
                var lst = context.DynamicQuery<DynamicVersion>(definingQuery).ToList();
                
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                var books = objectContext.CreateObjectSet<Book>();
                var queryBooks = from b in books
                    join d in dynQ on b.Id equals d.UpdatePackCount
                    select new
                    {
                        b.AuthorId,
                        d.DatabaseVersion
                    };

                var queryBooksTrace = ((ObjectQuery)queryBooks).ToTraceString();
                var lstBooks = queryBooks.ToList();
            }
        }
        
        [Fact]
        public void CanQueryTempTable()
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_dynamic (
	[DatabaseVersion] [VARCHAR](50) NOT NULL,
	[IsUpdating] [BIT] NOT NULL,
	[UpdatePackCount] [INT] NOT NULL,
);";
            using (var context = new MyContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                objectContext.Connection.Open();
                objectContext.ExecuteStoreCommand(sqlCreate);
                
                var dynQ = context.DynamicQuery<DynamicVersion>("TABLE:#t_dynamic");
                var strDynQ = ((ObjectQuery)dynQ).ToTraceString();
                var lst = dynQ.AsNoTracking().ToList();
                
                var books = objectContext.CreateObjectSet<Book>();
                var queryBooks = from b in books
                    join d in dynQ on b.Id equals d.UpdatePackCount
                    select new
                    {
                        b.AuthorId,
                        d.DatabaseVersion
                    };
                
                var queryBooksTrace = ((ObjectQuery)queryBooks).ToTraceString();
                var lstBooks = queryBooks.ToList();
            }
        }

        [Fact]
        public void CanInsertTempTable() => CanInsertTempTableInternal(false);

        [Fact]
        public void CanInsertTempTableWithCache() => CanInsertTempTableInternal(true);

        private void CanInsertTempTableInternal(bool withCache)
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_Books([Id] [INT] NOT NULL, [Title] [NVARCHAR](200) NULL, [AuthorId] [INT] NOT NULL);";
            using (var context = new MyContext())
            {
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    objectContext.Connection.Open();
                    objectContext.ExecuteStoreCommand(sqlCreate);

                    IQueryable<Author> authors = objectContext.CreateObjectSet<Author>();
                    var dynQ = context.DynamicQuery<TempBook>("TABLE:#t_Books");
                    var strDynQ = ((ObjectQuery)dynQ).ToTraceString();
                
                    var toInsert = authors.Where(
                            e => !e.Name.Contains("aaa")
                        )
                        .Take(20)
                        .Select(
                            e => new TempBook
                            {
                                Id = e.Id + 100500,
                                Title = "Book written by " + e.Name,
                                AuthorId = e.Id
                            });
                
                    var fromQuery = (ObjectQuery<TempBook>)toInsert;
                    var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(TempBook));
                    if (withCache)
                        options.UniqueSetName = "UUQ_t_Books";
                    var insertQuery = BatchDmlFactory.CreateBatchInsertDynamicTableQuery(fromQuery, "#t_Books", options, true);
                    var strInsert = insertQuery.ToTraceString();
                    var result = insertQuery.Execute();

                    tr.Rollback();
                }
            }
        }
        
        [Fact]
        public void CanInsertTempTableOmitProperties() => CanInsertTempTableOmitPropertiesInternal(false);

        [Fact]
        public void CanInsertTempTableOmitPropertiesWithCache() => CanInsertTempTableOmitPropertiesInternal(true);

        private void CanInsertTempTableOmitPropertiesInternal(bool withCache)
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_Books([Id] [INT] NOT NULL, [Title] [NVARCHAR](200) NULL, [AuthorId] [INT] NOT NULL);";
            using (var context = new MyContext())
            {
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    objectContext.Connection.Open();
                    objectContext.ExecuteStoreCommand(sqlCreate);

                    IQueryable<Author> authors = objectContext.CreateObjectSet<Author>();
                    var dynQ = context.DynamicQuery<TempBook>("TABLE:#t_Books");
                    var strDynQ = ((ObjectQuery)dynQ).ToTraceString();
                
                    var toInsert = authors.Where(
                            e => !e.Name.Contains("aaa")
                        )
                        .Take(20)
                        .Select(
                            e => new TempBook
                            {
                                Id = e.Id + 100500
                            });
                
                    var fromQuery = (ObjectQuery<TempBook>)toInsert;
                    var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(TempBook));
                    if (withCache)
                        options.UniqueSetName = "UUQ_t_Books";
                    var insertQuery = BatchDmlFactory.CreateBatchInsertDynamicTableQuery(fromQuery, "#t_Books", options, true);
                    var strInsert = insertQuery.ToTraceString();
                    var result = insertQuery.Execute();

                    tr.Rollback();
                }
            }
        }
        
        [Fact]
        public void CanInsertTempTableWithParameters() => CanInsertTempTableWithParametersInternal(false);
        
        [Fact]
        public void CanInsertTempTableWithCacheAndParameters() => CanInsertTempTableWithParametersInternal(true);
        
        private static void CanInsertTempTableWithParametersInternal(bool withCache)
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_Books([Id] [INT] NOT NULL, [Title] [NVARCHAR](200) NULL, [AuthorId] [INT] NOT NULL);";
            using (var context = new MyContext())
            {
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    objectContext.Connection.Open();
                    objectContext.ExecuteStoreCommand(sqlCreate);

                    IQueryable<Author> authors = objectContext.CreateObjectSet<Author>();
                    var dynQ = context.DynamicQuery<TempBook>("TABLE:#t_Books");
                    var strDynQ = ((ObjectQuery)dynQ).ToTraceString();

                    var addId = 100500;
                    var addTitle = "Book ";

                    var toInsert = authors.Where(
                            e => !e.Name.Contains("aaa")
                        )
                        .Take(20)
                        .Select(
                            e => new TempBook
                            {
                                Id = e.Id + addId,
                                Title = addTitle + "written by " + e.Name,
                                AuthorId = e.Id
                            });

                    var fromQuery = (ObjectQuery<TempBook>)toInsert;
                    var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(TempBook));
                    if (withCache)
                        options.UniqueSetName = "UUQ_t_Books";
                    var insertQuery = BatchDmlFactory.CreateBatchInsertDynamicTableQuery(fromQuery, "#t_Books", options, true);
                    var strInsert = insertQuery.ToTraceString();
                    var result = insertQuery.Execute();

                    tr.Rollback();
                }
            }
        }

        [Fact]
        public void CanUpdateTempTable()
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_Books([Id] [INT] NOT NULL, [Title] [NVARCHAR](200) NULL, [AuthorId] [INT] NOT NULL);";
            const string sqlInsert = @"INSERT " + @"INTO #t_Books
VALUES (1, 'Book 1', 1), (2, 'Book 2', 1), (3, 'Book 3', 2), (4, 'Book 4', 3)";
            using (var context = new MyContext())
            {
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    objectContext.Connection.Open();
                    objectContext.ExecuteStoreCommand(sqlCreate);
                    objectContext.ExecuteStoreCommand(sqlInsert);

                    var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(TempBook));
                    var tempBooks = context.DynamicQuery<TempBook>("TABLE:#t_Books", options);
                    
                    var toUpdate = tempBooks.Where(
                        e => !e.Title.Contains("aaa") && e.AuthorId > 1
                    );
                    
                    var fromQuery = (ObjectQuery<TempBook>)toUpdate;
                    
                    var updateQuery = BatchDmlFactory.CreateBatchUpdateDynamicTableQuery(
                        fromQuery,
                        options,
                        e => new TempBook()
                        {
                            Title = e.Title + " aaa",
                        }, true, 120);
                    var strToUpdate = updateQuery.ToTraceString();

                    var result = updateQuery.Execute();
                    
                    tr.Rollback();
                }
            }
        }
        
        [Fact]
        public void CanUpdateTempTableWithJoin()
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_Books([Id] [INT] NOT NULL, [Title] [NVARCHAR](200) NULL, [AuthorId] [INT] NOT NULL);";
            const string sqlInsert = @"INSERT " + @"INTO #t_Books
VALUES (1, 'Book 1', 1), (2, 'Book 2', 1), (3, 'Book 3', 2), (4, 'Book 4', 3)";
            using (var context = new MyContext())
            {
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    objectContext.Connection.Open();
                    objectContext.ExecuteStoreCommand(sqlCreate);
                    objectContext.ExecuteStoreCommand(sqlInsert);

                    IQueryable<Author> authors = objectContext.CreateObjectSet<Author>();
                    var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(TempBook));
                    var tempBooks = context.DynamicQuery<TempBook>("TABLE:#t_Books", options);
                    
                    var toUpdate = tempBooks.Join(authors, b => b.AuthorId, a => a.Id, (b, a) => new BatchDmlFactory.JoinTuple<TempBook, Author>{Entity = b, Source = a})
                        .Where(
                        e => !e.Entity.Title.Contains("aaa") && e.Source.Name != "Ralph"
                    );
                    
                    var fromQuery = (ObjectQuery<BatchDmlFactory.JoinTuple<TempBook, Author>>)toUpdate;
                    
                    var updateQuery = BatchDmlFactory.CreateBatchUpdateDynamicTableJoinQuery(
                        fromQuery,
                        options,
                        x => new TempBook()
                        {
                            Title = x.Entity.Title + " " + x.Source.Name,
                        }, true, 120);
                    var strToUpdate = updateQuery.ToTraceString();

                    var result = updateQuery.Execute();
                    
                    tr.Rollback();
                }
            }
        }
        
        [Fact]
        public void CanDeleteTempTable()
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_Books([Id] [INT] NOT NULL, [Title] [NVARCHAR](200) NULL, [AuthorId] [INT] NOT NULL);";
            const string sqlInsert = @"INSERT " + @"INTO #t_Books VALUES (1, 'Book 1', 1), (2, 'Book 2', 1), (3, 'Book 3', 2), (4, 'Book 4', 3)";
            using (var context = new MyContext())
            {
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    objectContext.Connection.Open();
                    objectContext.ExecuteStoreCommand(sqlCreate);
                    objectContext.ExecuteStoreCommand(sqlInsert);

                    var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(TempBook));
                    var tempBooks = context.DynamicQuery<TempBook>("TABLE:#t_Books", options);
                    
                    var toDelete = tempBooks.Where(
                        e => !e.Title.Contains("aaa") && e.AuthorId > 1
                    );
                    
                    var fromQuery = (ObjectQuery<TempBook>)toDelete;
                    
                    var deleteQuery = BatchDmlFactory.CreateBatchDeleteDynamicTableQuery(
                        fromQuery, options, true, 120);
                    var strToDelete = deleteQuery.ToTraceString();

                    var result = deleteQuery.Execute();
                    
                    tr.Rollback();
                }
            }
        }
        
        [Fact]
        public void CanDeleteTempTableWithJoin()
        {
            const string sqlCreate = @"CREATE " + @"TABLE #t_Books([Id] [INT] NOT NULL, [Title] [NVARCHAR](200) NULL, [AuthorId] [INT] NOT NULL);";
            const string sqlInsert = @"INSERT " + @"INTO #t_Books VALUES (1, 'Book 1', 1), (2, 'Book 2', 1), (3, 'Book 3', 2), (4, 'Book 4', 3)";
            using (var context = new MyContext())
            {
                using (var tr = context.Database.BeginTransaction())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    objectContext.Connection.Open();
                    objectContext.ExecuteStoreCommand(sqlCreate);
                    objectContext.ExecuteStoreCommand(sqlInsert);

                    IQueryable<Author> authors = objectContext.CreateObjectSet<Author>();
                    var options = DynamicQueryUtils.CreateDynamicQueryOptions(typeof(TempBook));
                    var tempBooks = context.DynamicQuery<TempBook>("TABLE:#t_Books", options);
                    
                    var toDelete = tempBooks.Join(authors, b => b.AuthorId, a => a.Id, (b, a) => new BatchDmlFactory.JoinTuple<TempBook, Author>{Entity = b, Source = a})
                        .Where(
                            e => !e.Entity.Title.Contains("aaa") && e.Source.Name != "Ralph"
                        );
                    
                    var fromQuery = (ObjectQuery<BatchDmlFactory.JoinTuple<TempBook, Author>>)toDelete;
                    
                    var deleteQuery = BatchDmlFactory.CreateBatchDeleteDynamicTableJoinQuery<BatchDmlFactory.JoinTuple<TempBook, Author>, TempBook>(
                        fromQuery, options, true, 120);
                    var strToDelete = deleteQuery.ToTraceString();

                    var result = deleteQuery.Execute();
                    
                    tr.Rollback();
                }
            }
        }
    }
}
