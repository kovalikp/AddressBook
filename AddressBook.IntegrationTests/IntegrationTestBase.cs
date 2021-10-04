// Copyright (c) Pavol Kovalik.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Nito.AsyncEx;
using Xunit;

namespace AddressBook
{
    public class IntegrationTestBase : IAsyncLifetime
    {
        private static readonly AsyncLock s_mutex = new();

        private static bool s_initialized;

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            if (s_initialized)
                return;

            using (await s_mutex.LockAsync())
            {
                if (s_initialized)
                    return;

                await DatabaseFixture.ResetCheckpoint();

                s_initialized = true;
            }
        }
    }
}
