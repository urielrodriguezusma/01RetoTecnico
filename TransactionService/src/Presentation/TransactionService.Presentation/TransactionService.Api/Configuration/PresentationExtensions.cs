using TransactionService.Api.Configuration.Extensions;
using TransactionService.Api.ExceptionHandlers;
using TransactionService.Application.Configuration;
using TransactionService.Persistence.Sql.Configuration;

namespace TransactionService.Api.Configuration;

public static class PresentationExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPresentationServices(configuration);
        services.AddApplicationServices();
        services.AddPersistenceSql(configuration);
    }

    internal static void AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        services.AddMassTransitConfiguration(configuration);

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
