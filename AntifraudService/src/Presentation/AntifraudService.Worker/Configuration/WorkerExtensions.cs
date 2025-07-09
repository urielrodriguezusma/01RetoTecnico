using AntifraudService.Application.Configuration;
using AntifraudService.PersistenceSql.Configuration;

namespace AntifraudService.Worker.Configuration;
public static class WorkerExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransitConfiguration(configuration);
        services.AddApplication();
        services.AddPersistenceSql(configuration);
    }
}
