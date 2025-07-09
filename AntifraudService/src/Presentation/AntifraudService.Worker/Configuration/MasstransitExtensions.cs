using AntifraudService.Domain;
using AntifraudService.Infrastructure.Configurations.Options;
using AntifraudService.Persistence.Sql;
using MassTransit;
using System.Text.Json.Serialization;

namespace AntifraudService.Worker.Configuration;
public static class MasstransitExtensions
{
    public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var busOptions = configuration.GetSection("BusConfiguration").Get<BusConfigurationOptions>();
        serviceCollection.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter($"{Globals.ServiceName.ToLowerInvariant()}{Globals.Separator}"));
            x.AddConsumers(typeof(Program).Assembly);
            x.AddEntityFrameworkOutbox<AntifraudDbContext>(o =>
            {
                o.UseSqlServer();
                o.DisableInboxCleanupService();
            });

            if (busOptions!.RabbitMQEnabled)
                ConfigureWithRabbitMq(x, busOptions);
            else
                ConfigureWithKafka(x, busOptions);
        });

        return serviceCollection;
    }

    private static void ConfigureWithRabbitMq(IBusRegistrationConfigurator x,
    BusConfigurationOptions busOptions)
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(busOptions.RabbitMQConnectionString);

            cfg.ConfigureJsonSerializerOptions(serializerOptions =>
            {
                serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                return serializerOptions;
            });
            //cfg.UseConsumeFilter(typeof(RetriveCustomHeaderConsumeFilter<>), context);
            cfg.ConfigureEndpoints(context);
            cfg.UseKillSwitch(options => options
               .SetActivationThreshold(10)
               .SetTripThreshold(0.15)
               .SetRestartTimeout(m: 1));
        });
    }

    private static void ConfigureWithKafka(IBusRegistrationConfigurator x,
        BusConfigurationOptions busOptions)
    {
        x.AddRider(rider =>
        {
            rider.UsingKafka((context, k) =>
            {
                k.Host(busOptions.KafkaConnectionString);
            });
        });
    }
}
