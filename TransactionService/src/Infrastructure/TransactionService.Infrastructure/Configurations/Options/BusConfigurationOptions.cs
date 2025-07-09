namespace TransactionService.Infrastructure.Configurations.Options;
public class BusConfigurationOptions
{
    public bool RabbitMQEnabled { get; set; }
    public string? RabbitMQConnectionString { get; set; }
    public string? KafkaConnectionString { get; set; }
}
