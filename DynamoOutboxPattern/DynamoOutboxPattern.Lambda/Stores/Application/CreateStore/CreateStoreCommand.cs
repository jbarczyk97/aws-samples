using DynamoOutboxPattern.Lambda.Stores.Domain;

namespace DynamoOutboxPattern.Lambda.Stores.Application.CreateStore;

public record CreateStoreCommand(string Name);

public class CreateStoreCommandHandler
{
    private readonly IStoreRepository _storeRepository;

    public CreateStoreCommandHandler(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task HandleAsync(CreateStoreCommand command)
    {
        var store = Store.New(command.Name);
        await _storeRepository.CreateAsync(store);
    }
}
