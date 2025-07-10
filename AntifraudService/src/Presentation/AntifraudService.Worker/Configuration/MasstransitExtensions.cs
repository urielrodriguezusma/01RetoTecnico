using AntifraudService.Domain;
using AntifraudService.Infrastructure.Configurations.Options;
using AntifraudService.Persistence.Sql;
using AntifraudService.Worker.Transactions.Consumers;
using MassTransit;
using System.Text.Json.Serialization;
using TransactionService.Messages.Transaction;

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
                k.TopicEndpoint<TransactionCreated>("transaction-created", "transfer-group", d =>
                {
                    d.ConfigureConsumer<OnTransactionCreatedThenValidateStatus>(context);
                });

            });

            rider.AddConsumer<OnTransactionCreatedThenValidateStatus>(context =>
            {
                context.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            });
        });
    }
}
