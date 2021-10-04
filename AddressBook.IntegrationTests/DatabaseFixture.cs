using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AddressBook.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace AddressBook
{
    public class DatabaseFixture
    {
        private static readonly Checkpoint s_checkpoint;
        private static readonly IConfigurationRoot s_configuration;
        private static readonly IServiceScopeFactory s_scopeFactory;

        private static int s_courseNumber = 1;

        static DatabaseFixture()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            s_configuration = builder.Build();

            Startup? startup = new(s_configuration);
            ServiceCollection? services = new();
            startup.ConfigureServices(services);
            ServiceProvider provider = services.BuildServiceProvider();
            s_scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            s_checkpoint = new Checkpoint();
        }

        public static Task ResetCheckpoint() => s_checkpoint.Reset(s_configuration.GetConnectionString("AddressBook"));

        public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (IServiceScope scope = s_scopeFactory.CreateScope())
            {
                AddressBookContext db = scope.ServiceProvider.GetRequiredService<AddressBookContext>();
                await using IDbContextTransaction transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
                try
                {
                    await action(scope.ServiceProvider).ConfigureAwait(false);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (IServiceScope scope = s_scopeFactory.CreateScope())
            {
                AddressBookContext db = scope.ServiceProvider.GetRequiredService<AddressBookContext>();
                await using IDbContextTransaction transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
                try
                {
                    T result = await action(scope.ServiceProvider).ConfigureAwait(false);
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public static Task ExecutedbAsync(Func<AddressBookContext, Task> action)
            => ExecuteScopeAsync(sp => action(sp.GetRequiredService<AddressBookContext>()));

        public static Task ExecutedbAsync(Func<AddressBookContext, IMediator, Task> action)
            => ExecuteScopeAsync(sp => action(sp.GetRequiredService<AddressBookContext>(), sp.GetRequiredService<IMediator>()));

        public static Task<T> ExecutedbAsync<T>(Func<AddressBookContext, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetRequiredService<AddressBookContext>()));

        public static Task<T> ExecutedbAsync<T>(Func<AddressBookContext, IMediator, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetRequiredService<AddressBookContext>(), sp.GetRequiredService<IMediator>()));

        public static Task InsertAsync<T>(params T[] entities) where T : class
        {
            return ExecutedbAsync(db =>
            {
                foreach (T entity in entities)
                {
                    db.Set<T>().Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return ExecutedbAsync(db =>
            {
                db.Set<TEntity>().Add(entity);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
            where TEntity : class
            where TEntity2 : class
        {
            return ExecutedbAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
            where TEntity : class
            where TEntity2 : class
            where TEntity3 : class
        {
            return ExecutedbAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3, TEntity4 entity4)
            where TEntity : class
            where TEntity2 : class
            where TEntity3 : class
            where TEntity4 : class
        {
            return ExecutedbAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);
                db.Set<TEntity4>().Add(entity4);

                return db.SaveChangesAsync();
            });
        }

        public static Task<T> FindAsync<T>(params object[] keyValues)
            where T : class
        {
            return ExecutedbAsync(async db => await db.Set<T>().FindAsync(keyValues));
        }

        public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                IMediator mediator = sp.GetRequiredService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                IMediator mediator = sp.GetRequiredService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static int NextCourseNumber() => Interlocked.Increment(ref s_courseNumber);
    }
}
