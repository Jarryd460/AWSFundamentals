using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

internal sealed class SqsMessager : ISqsMessager
{
    private readonly IAmazonSQS _amazonSQS;
    private readonly IOptions<Queue> _queueSettings;
    private string? _queueUrl;

    public SqsMessager(IAmazonSQS amazonSQS, IOptions<Queue> queueSettings)
    {
        _amazonSQS = amazonSQS;
        _queueSettings = queueSettings;
    }

    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        string queueUrl = await GetQueueUrlAsync().ConfigureAwait(false);

        var sendMessageRequest = new SendMessageRequest()
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "MessageType", new MessageAttributeValue()
                        {
                            DataType = "String",
                            StringValue = typeof(T).Name
                        }
                    }
                }
        };

        return await _amazonSQS.SendMessageAsync(sendMessageRequest).ConfigureAwait(false);
    }

    private async Task<string> GetQueueUrlAsync()
    {
        if (_queueUrl is not null)
        {
            return _queueUrl;
        }

        var queueUrlResponse = await _amazonSQS.GetQueueUrlAsync(_queueSettings.Value.Name).ConfigureAwait(false);

        _queueUrl = queueUrlResponse.QueueUrl;

        return _queueUrl;
    }
}
