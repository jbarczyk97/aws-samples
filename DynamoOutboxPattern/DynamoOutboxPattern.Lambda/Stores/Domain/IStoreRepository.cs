namespace DynamoOutboxPattern.Lambda.Stores.Domain;

public interface IStoreRepository
{
    Task CreateAsync(Store store);
}
