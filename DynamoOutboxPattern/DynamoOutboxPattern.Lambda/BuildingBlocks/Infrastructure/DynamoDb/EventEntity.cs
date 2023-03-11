using DynamoOutboxPattern.Lambda.BuildingBlocks.Domain;
using DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb.Converters;
using EfficientDynamoDb.Attributes;
using Newtonsoft.Json.Linq;

namespace DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb;

public class EventEntity : BaseEntity
{
    public EventEntity(IIntegrationEvent @event)
    {
        Id = @event.Id;
        EventType = @event.GetType().Name;
        CreatedAt = @event.CreatedAt;
        Detail = JObject.FromObject(@event);
    }

    // ReSharper disable once UnusedMember.Local
    public EventEntity() { }

    [DynamoDbProperty("Type")]
    public override string Type => "EVENT";

    [DynamoDbProperty("PK", DynamoDbAttributeType.PartitionKey)]
    public override string Pk => $"EVENT";

    [DynamoDbProperty("SK", DynamoDbAttributeType.SortKey)]
    public override string Sk => $"EVENT#{CreatedAt:s}#{Id}";

    [DynamoDbProperty("Id")]
    public Guid Id { get; }
    
    [DynamoDbProperty("CreatedAt")]
    public DateTime CreatedAt { get; }

    [DynamoDbProperty("EventType")]
    public string EventType { get; }

    [DynamoDbProperty("Detail", typeof(JTokenConverter))]
    public JObject Detail { get; }
}