// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Migrations
{
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;

#if USES_MICROSOFT_DATA_SQLCLIENT
    using SqlServerMigrationSqlGenerator = System.Data.Entity.SqlServer.MicrosoftSqlServerMigrationSqlGenerator;
#else
    using SqlServerMigrationSqlGenerator = System.Data.Entity.SqlServer.SqlServerMigrationSqlGenerator;
#endif

    internal class CustomSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(MigrationOperation migrationOperation)
        {
            var commentOperation = migrationOperation as CommentOperation;
            if (commentOperation != null)
            {
                Statement(string.Format("-- {0}", commentOperation.Text));
            }
            else
            {
                base.Generate(migrationOperation);
            }
        }
    }
}
