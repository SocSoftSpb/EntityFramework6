// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core.Objects
{
    using System.Collections.Generic;
    using System.Data.Entity.Utilities;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class EntityFunctionsTests : TestBase
    {
        [Fact]
        public void All_EntityFunctions_are_attributed_with_DbFunctionAttribute_except_like_and_unicode_methods()
        {
#pragma warning disable 612,618
            var entityFunctions = typeof(EntityFunctions).GetDeclaredMethods().ToList();
#pragma warning restore 612,618
            Assert.True(entityFunctions.Count >= 95); // Just make sure Reflection is returning what we expect

            var excludedNames = new HashSet<string> { "Like", "LikeCommon", "AsUnicode", "AsNonUnicode", "FtContains" };
            foreach (var function in entityFunctions.Where(f => f.IsPublic && !excludedNames.Contains(f.Name)))
            {
                Assert.NotNull(function.GetCustomAttributes<DbFunctionAttribute>(inherit: false).FirstOrDefault());
            }        }

        [Fact]
        public void All_DbFunctions_are_also_included_in_EntityFunctions()
        {
            var dbFunctions = typeof(DbFunctions).GetDeclaredMethods().Where(f => f.IsPublic).ToList();
#pragma warning disable 612,618
            var entityFunctions = typeof(EntityFunctions).GetDeclaredMethods().Where(f => f.IsPublic).ToList();
#pragma warning restore 612,618

            // foreach (var f in dbFunctions)
            // {
            //     var f1 = entityFunctions.FirstOrDefault(e => e.ToString() == f.ToString());
            //     if (f1 == null)
            //     {
            //
            //     }
            // }

            Assert.Equal(dbFunctions.Count(), entityFunctions.Count());
            Assert.True(dbFunctions.Count() >= 95); // Just make sure Reflection is returning what we expect

            foreach (var function in dbFunctions)
            {
                var dbFunction = function;
                Assert.Equal(
                    1, entityFunctions.Count(
                        f => f.Name == dbFunction.Name
                             && f.GetParameters()
                                 .Select(p => p.ParameterType)
                                 .SequenceEqual(dbFunction.GetParameters().Select(p => p.ParameterType))));
            }
        }
    }
}
