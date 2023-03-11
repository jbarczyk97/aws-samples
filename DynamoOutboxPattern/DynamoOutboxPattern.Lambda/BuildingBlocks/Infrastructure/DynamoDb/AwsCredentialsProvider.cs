using Amazon.Runtime;
using EfficientDynamoDb.Configs;

namespace DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb;

// ReSharper disable CommentTypo
// Copy & pasted from EfficientDynamoDb.Credentials.AWSSDK to avoid referencing latest AWSSDK.Core
// ReSharper restore CommentTypo
public class AwsCredentialsProvider : IAwsCredentialsProvider
{
    private readonly AWSCredentials _awsCredentials;

    public AwsCredentialsProvider(AWSCredentials awsCredentials)
    {
        _awsCredentials = awsCredentials;
    }

    public async ValueTask<AwsCredentials> GetCredentialsAsync(CancellationToken cancellationToken = default)
    {
        var immutableCredentials = await _awsCredentials.GetCredentialsAsync().ConfigureAwait(false);
        return new AwsCredentials(immutableCredentials.AccessKey, immutableCredentials.SecretKey, immutableCredentials.Token);
    }
}