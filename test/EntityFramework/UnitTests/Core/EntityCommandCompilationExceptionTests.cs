// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Core
{
    using Xunit;

    public class EntityCommandCompilationExceptionTests
    {
        [Fact]
        public void Constructors_can_be_passed_null_or_empty_message_without_throwing()
        {
#if NET10_0_OR_GREATER
            const string message = "Data Exception.";
#else
            const string message = "System.Data.Entity.Core.EntityCommandCompilationException";
#endif
            Assert.True(new EntityCommandCompilationException(null).Message.Contains(
                message));

            Assert.Equal("", new EntityCommandCompilationException("").Message);
            Assert.Equal(" ", new EntityCommandCompilationException(" ").Message);

            Assert.True(new EntityCommandCompilationException(null, new Exception()).Message.Contains(
                message));

            Assert.Equal("", new EntityCommandCompilationException("", new Exception()).Message);
            Assert.Equal(" ", new EntityCommandCompilationException(" ", new Exception()).Message);

            Assert.Null(new EntityCommandCompilationException("Foo", null).InnerException);
        }
    }
}
