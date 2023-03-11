using DynamoOutboxPattern.Lambda.BuildingBlocks.Domain;

namespace DynamoOutboxPattern.Lambda.Stores.Domain;

public class StoreCreatedEvent : IntegrationEventBase
{
    public StoreCreatedEvent(Guid storeId, string storeName)
    {
        StoreId = storeId;
        StoreName = storeName;
    }

    public Guid StoreId { get; }
    public string StoreName { get; }
}
