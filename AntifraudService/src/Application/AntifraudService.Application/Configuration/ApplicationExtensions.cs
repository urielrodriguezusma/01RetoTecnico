using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AntifraudService.Application.Configuration;
public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(executingAssembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ApplicationExtensions).Assembly);
        });
        services.AddAutoMapper(executingAssembly);
    }
}
