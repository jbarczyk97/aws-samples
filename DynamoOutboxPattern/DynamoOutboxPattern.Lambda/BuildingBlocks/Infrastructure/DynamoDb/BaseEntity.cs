using EfficientDynamoDb.Attributes;

namespace DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb;

[DynamoDbTable("DynamoDbTable")]
public abstract class BaseEntity
{
    [DynamoDbProperty("Type")]
    public abstract string Type { get; }

    [DynamoDbProperty("PK", DynamoDbAttributeType.PartitionKey)]
    public abstract string Pk { get; }

    [DynamoDbProperty("SK", DynamoDbAttributeType.SortKey)]
    public abstract string Sk { get; }
}