using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TransactionService.Application.Utils.Behaviours;

namespace TransactionService.Application.Configuration;
public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(executingAssembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ApplicationExtensions).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddAutoMapper(executingAssembly);
    }
}
