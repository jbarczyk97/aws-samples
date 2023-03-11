namespace DynamoOutboxPattern.Lambda.BuildingBlocks.Domain;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
}

public abstract class IntegrationEventBase : IIntegrationEvent
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }

    public IntegrationEventBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}