// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Migrations
{
    using System.Data.Entity.Functionals.Utilities;
    using System.Data.Entity.Migrations.CustomOperations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Data.Entity.SqlServer;

#if USES_MICROSOFT_DATA_SQLCLIENT
    using SqlProviderServices = System.Data.Entity.SqlServer.MicrosoftSqlProviderServices;
#else
    using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;
#endif


    using Xunit;

    public class CustomOperationScenarios : FunctionalTestBase
    {
        [Fact]
        public void Can_use_custom_operations()
        {
            var configuration = new DbMigrationsConfiguration<EmptyModel>
                {
                    MigrationsAssembly = typeof(CustomOperationMigration).Assembly(),
                    MigrationsNamespace = typeof(CustomOperationMigration).Namespace
                };
            configuration.SetSqlGenerator(SqlProviderServices.ProviderInvariantName, new CustomSqlGenerator());

            var migrator = new DbMigrator(configuration);
            var scriptor = new MigratorScriptingDecorator(migrator);

            var sql = scriptor.ScriptUpdate(DbMigrator.InitialDatabase, null);

            Assert.Contains("-- This is a test.", sql);
        }
    }
}
