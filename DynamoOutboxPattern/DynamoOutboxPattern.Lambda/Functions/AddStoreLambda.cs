using Amazon.Lambda.Core;

namespace DynamoOutboxPattern.Lambda.Functions;

public class AddStoreLambda
{
    public Task Run(ILambdaContext context)
    {
        context.Logger.LogInformation("Hello from Lambda");
        return Task.CompletedTask;
    }
}
