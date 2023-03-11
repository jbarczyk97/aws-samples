using Amazon.Lambda.Core;
using Amazon.Runtime;
using DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb;
using DynamoOutboxPattern.Lambda.Stores.Application.CreateStore;
using DynamoOutboxPattern.Lambda.Stores.Infrastructure.DynamoDb;
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;

namespace DynamoOutboxPattern.Lambda.Functions;

public class AddStoreRequest
{
    public string StoreName { get; set; } = null!;
}

public class AddStoreLambda
{
    private readonly CreateStoreCommandHandler _handler;

    public AddStoreLambda()
    {
        var credentialsProvider = new AwsCredentialsProvider(FallbackCredentialsFactory.GetCredentials());
        var internalConfig =
            new DynamoDbContextConfig(RegionEndpoint.Create(Environment.GetEnvironmentVariable("AwsRegion")!), credentialsProvider)
                { TableNamePrefix = $"{Environment.GetEnvironmentVariable("DatabaseStackName")}" + "-" };
        _handler = new CreateStoreCommandHandler(new StoreRepository(new DynamoDbContext(internalConfig)));
    }

    public async Task Run(AddStoreRequest request, ILambdaContext context)
    {
        if (string.IsNullOrEmpty(request.StoreName))
        {
            throw new ArgumentException("Store name is required", nameof(request.StoreName));
        }

        await _handler.HandleAsync(new CreateStoreCommand(request.StoreName));
    }
}
