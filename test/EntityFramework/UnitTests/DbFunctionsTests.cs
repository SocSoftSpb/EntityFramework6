// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity
{
    using System.Collections.Generic;
    using System.Data.Entity.Utilities;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class DbFunctionsTests : TestBase
    {
        [Fact]
        public void All_DbFunctions_are_attributed_with_DbFunctionAttribute_except_like_and_unicode_methods()
        {
            var entityFunctions = typeof(DbFunctions).GetDeclaredMethods().Where(f => f.IsPublic).ToList();
            Assert.True(entityFunctions.Count() >= 95); // Just make sure Reflection is returning what we expect

            HashSet<string> exclusions = ["Like", "AsUnicode", "AsNonUnicode", "LikeCommon", "FtContains"];

            foreach (var function in entityFunctions.Where(f => !exclusions.Contains(f.Name)))
            {
                Assert.NotNull(function.GetCustomAttributes<DbFunctionAttribute>(inherit: false).FirstOrDefault());
            }
        }
    }
}
