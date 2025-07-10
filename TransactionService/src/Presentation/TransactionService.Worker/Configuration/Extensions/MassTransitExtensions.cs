using AntifraudService.Messages.Transaction;
using MassTransit;
using System.Text.Json.Serialization;
using TransactionService.Domain;
using TransactionService.Infrastructure.Configurations.Options;
using TransactionService.Persistence.Sql;
using TransactionService.Worker.Transactions.Consumers;

namespace TransactionService.Worker.Configuration.Extensions;
public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection serviceCollection,
    IConfiguration configuration)
    {
        var busOptions = configuration.GetSection("BusConfiguration").Get<BusConfigurationOptions>();
        serviceCollection.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter($"{Globals.ServiceName.ToLowerInvariant()}{Globals.Separator}"));
            x.AddConsumers(typeof(Program).Assembly);
            x.AddEntityFrameworkOutbox<TransactionDbContext>(o =>
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
                k.TopicEndpoint<TransactionValidated>("transaction-validated", "transfer-group", d =>
                {
                    d.ConfigureConsumer<OnTransactionValidatedThenUpdateTransactionStatus>(context);
                });
            });

            rider.AddConsumer<OnTransactionValidatedThenUpdateTransactionStatus>(context =>
            {
                context.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            });
        });
    }
}
