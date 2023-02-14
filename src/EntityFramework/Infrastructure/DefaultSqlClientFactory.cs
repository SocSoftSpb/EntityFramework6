// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Infrastructure
{
    using System.Data.Common;
    using System.Data.Entity.Core.Common;
    using System.Threading;

    public static class DefaultSqlClientFactory
    {
        private sealed class ProviderState
        {
            public ProviderState(DbProviderFactory factory, DbProviderServices services)
            {
                Factory = factory;
                Services = services;
            }

            public DbProviderFactory Factory { get; }

            public DbProviderServices Services { get; }
        }

        private static ProviderState _providerState;

        public static DbProviderFactory ProviderFactory
        {
            get { return Volatile.Read(ref _providerState)?.Factory; }
        }

        public static DbProviderServices ProviderServices
        {
            get { return Volatile.Read(ref _providerState)?.Services; }
        }

        public static void TryInitialize(DbProviderFactory providerFactory, DbProviderServices providerServices)
        {
            Interlocked.CompareExchange(
                ref _providerState,
                new ProviderState(providerFactory, providerServices),
                null);
        }
    }
}
