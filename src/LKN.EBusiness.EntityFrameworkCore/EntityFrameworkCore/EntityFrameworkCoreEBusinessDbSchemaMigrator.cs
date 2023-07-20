using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LKN.EBusiness.Data;
using Volo.Abp.DependencyInjection;

namespace LKN.EBusiness.EntityFrameworkCore;

public class EntityFrameworkCoreEBusinessDbSchemaMigrator
    : IEBusinessDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEBusinessDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the EBusinessDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EBusinessDbContext>()
            .Database
            .MigrateAsync();
    }
}
