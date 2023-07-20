using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LKN.EBusiness.Data;

/* This is used if database provider does't define
 * IEBusinessDbSchemaMigrator implementation.
 */
public class NullEBusinessDbSchemaMigrator : IEBusinessDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
