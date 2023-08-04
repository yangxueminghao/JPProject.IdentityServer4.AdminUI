using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Jp.Database.Context;
using JPProject.Admin.EntityFramework.Repository.Context;
using JPProject.EntityFrameworkCore.MigrationHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Api.Configuration
{
    public static class AdminUiMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context EventStoreContext -output Data/Migrations
        /// Dotnet CLI: dotnet ef migrations add DbInit -c EventStoreContext -o Data/Migrations
        /// </summary>
        public static async Task EnsureSeedData(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            var ssoContext = services.GetRequiredService<JpProjectAdminUiContext>();
            //var appliedMigrationsScripts = ssoContext.Database.GetAppliedMigrations().ToList();
            //var migrationsScripts = ssoContext.Database.GetMigrations().ToList();
            //var pendingMigrationsScripts = ssoContext.Database.GetPendingMigrations().ToList();
            //var generateCreateMigrationsScripts = ssoContext.Database.GenerateCreateScript();
            //ssoContext.Database.Migrate();

            //ssoContext.Database.ExecuteSqlRaw(generateCreateMigrationsScripts);

            if (!ssoContext.Clients.Any())
            {
                foreach (var client in Config.GetClients(false))
                {

                    ssoContext.Clients.Add(client.ToEntity());
                    ssoContext.SaveChanges();
                }
            }
            if (!ssoContext.ApiResources.Any())
            {
                foreach (var api in Config.GetApis())
                {
                    ssoContext.ApiResources.Add(api.ToEntity());
                    ssoContext.SaveChanges();
                }
            }
            if (!ssoContext.IdentityResources.Any())
            {
                foreach (var identityResource in Config.GetIdentityResources())
                {
                    ssoContext.IdentityResources.Add(identityResource.ToEntity());
                    ssoContext.SaveChanges();
                }
            }

            Log.Information("Check if database contains Client (ConfigurationDbStore) table");
            await DbHealthChecker.WaitForTable<Client>(ssoContext);

            Log.Information("Check if database contains PersistedGrant (PersistedGrantDbStore) table");
            await DbHealthChecker.WaitForTable<PersistedGrant>(ssoContext);
            Log.Information("Checks done");

            var eventStoreDb = serviceScope.ServiceProvider.GetRequiredService<EventStoreContext>();
            await eventStoreDb.Database.EnsureCreatedAsync();
        }
    }
}
