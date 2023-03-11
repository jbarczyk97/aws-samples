using DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb;
using DynamoOutboxPattern.Lambda.Stores.Domain;
using EfficientDynamoDb;
using EfficientDynamoDb.Operations.TransactWriteItems.Builders;

namespace DynamoOutboxPattern.Lambda.Stores.Infrastructure.DynamoDb;

public class StoreRepository : IStoreRepository
{
    private readonly IDynamoDbContext _dbContext;

    public StoreRepository(IDynamoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CreateAsync(Store store)
    {
        var storeEntity = new StoreEntity(store.Id, store.Name);
        var operations = new List<ITransactWriteItemBuilder> { Transact.PutItem(storeEntity) };
        var events = store.Events.Select(x => new EventEntity(x));
        operations.AddRange(events.Select(Transact.PutItem));
        return _dbContext.TransactWrite().WithItems(operations).ExecuteAsync();
    }
}
