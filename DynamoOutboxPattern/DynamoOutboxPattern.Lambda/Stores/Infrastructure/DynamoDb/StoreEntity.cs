using DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb;
using EfficientDynamoDb.Attributes;

namespace DynamoOutboxPattern.Lambda.Stores.Infrastructure.DynamoDb;

public class StoreEntity : BaseEntity
{
    public StoreEntity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    // ReSharper disable once UnusedMember.Local
    public StoreEntity() {}

    [DynamoDbProperty("Type")]
    public override string Type => "STORE";

    [DynamoDbProperty("PK", DynamoDbAttributeType.PartitionKey)]
    public override string Pk => $"Store#{Id}";

    [DynamoDbProperty("SK", DynamoDbAttributeType.SortKey)]
    public override string Sk => $"Store#{Id}";

    [DynamoDbProperty("Id")]
    public Guid Id { get; }

    [DynamoDbProperty("Name")]
    public string Name { get; }
}