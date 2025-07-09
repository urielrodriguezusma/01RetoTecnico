using TransactionService.Application.Configuration;
using TransactionService.Persistence.Sql.Configuration;
using TransactionService.Worker.Configuration.Extensions;

namespace TransactionService.Worker.Configuration;
public static class WorkerExtensions
{
    public static void ConfigureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPresentation(configuration);
        services.AddApplicationServices();
        services.AddPersistenceSql(configuration);
    }

    private static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransitConfiguration(configuration);
    }
}
