// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.EntityClient
{
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Infrastructure.DependencyResolution;
#if USES_MICROSOFT_DATA_SQLCLIENT
    using SqlProviderServices = System.Data.Entity.SqlServer.MicrosoftSqlProviderServices;
#else
    using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;
#endif
    using Moq;
    using Xunit;

    public class DependencyResolverTests : FunctionalTestBase
    {
        public DependencyResolverTests()
        {
            CreateMetadataFilesForSimpleModel();
        }

        [Fact]
        public void DependencyResolver_used_to_resolve_DbProviderServices()
        {
            const string query = @"
SELECT VALUE p 
FROM CodeFirstContainer.Products AS p
WHERE p.ID > 3";

            var mockResolver = new Mock<IDbDependencyResolver>();
            mockResolver
                .Setup(
                    r => r.GetService(
                        It.Is<Type>(t => t == typeof(DbProviderServices)),
                        It.Is<string>(s => s == SqlProviderServices.ProviderInvariantName)))
                .Returns(SqlProviderServices.Instance);

            using (var connection = new EntityConnection(SimpleModelEntityConnectionString))
            {
                connection.Open();

                new EntityCommand(query, connection, mockResolver.Object)
                    .ExecuteReader(CommandBehavior.SequentialAccess);
            }

            mockResolver.Verify(m => m.GetService(typeof(DbProviderServices), SqlProviderServices.ProviderInvariantName), Times.Once());
        }
    }
}