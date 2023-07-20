using System.Threading.Tasks;

namespace LKN.EBusiness.Data;

public interface IEBusinessDbSchemaMigrator
{
    Task MigrateAsync();
}
