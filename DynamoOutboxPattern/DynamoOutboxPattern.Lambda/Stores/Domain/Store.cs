using DynamoOutboxPattern.Lambda.BuildingBlocks.Domain;

namespace DynamoOutboxPattern.Lambda.Stores.Domain;

public class Store
{
    private readonly List<IIntegrationEvent> _events = new();

    private Store(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<IIntegrationEvent> Events => _events.AsReadOnly();

    public static Store New(string name)
    {
        var store = new Store(Guid.NewGuid(), name);
        store._events.Add(new StoreCreatedEvent(store.Id, store.Name));
        return store;
    }
}
