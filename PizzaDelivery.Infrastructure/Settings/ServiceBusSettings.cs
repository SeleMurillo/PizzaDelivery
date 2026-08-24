namespace PizzaDelivery.Infrastructure.Settings;

public class ServiceBusSettings
{
    public const string SectionName = "ServiceBus";  // Nombre de la sección en appsettings.json

    public string ConnectionString { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
}
