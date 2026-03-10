// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core
{
    using Xunit;

    public class EntityCommandExecutionExceptionTests
    {
        [Fact]
        public void Constructors_can_be_passed_null_or_empty_message_without_throwing()
        {
#if NET10_0_OR_GREATER
            const string message = "Data Exception.";
#else
            const string message = "System.Data.Entity.Core.EntityCommandExecutionException";
#endif
            Assert.Contains(
                message,
                new EntityCommandExecutionException(null).Message);

            Assert.Equal("", new EntityCommandExecutionException("").Message);
            Assert.Equal(" ", new EntityCommandExecutionException(" ").Message);

            Assert.Contains(
                message, 
                new EntityCommandExecutionException(null, new Exception()).Message);

            Assert.Equal("", new EntityCommandExecutionException("", new Exception()).Message);
            Assert.Equal(" ", new EntityCommandExecutionException(" ", new Exception()).Message);

            Assert.Null(new EntityCommandExecutionException("Foo", null).InnerException);
        }
    }
}
