using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging;

internal sealed class SnsMessager : ISnsMessager
{
    private readonly IAmazonSimpleNotificationService _amazonSNS;
    private readonly IOptions<TopicSettings> _topicSettings;
    private string? _topicARN;

    public SnsMessager(IAmazonSimpleNotificationService amazonSNS, IOptions<TopicSettings> topicSettings)
    {
        _amazonSNS = amazonSNS;
        _topicSettings = topicSettings;
    }

    public async Task<PublishResponse> PublishMessageAsync<T>(T message)
    {
        string topicARN = await GetTopicARNAsync().ConfigureAwait(false);

        var sendMessageRequest = new PublishRequest()
        {
            TopicArn = topicARN,
            Message = JsonSerializer.Serialize(message),
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

        return await _amazonSNS.PublishAsync(sendMessageRequest).ConfigureAwait(false);
    }

    private async ValueTask<string> GetTopicARNAsync()
    {
        if (_topicARN is not null)
        {
            return _topicARN;
        }

        var queueUrlResponse = await _amazonSNS.FindTopicAsync(_topicSettings.Value.Name).ConfigureAwait(false);

        _topicARN = queueUrlResponse.TopicArn;

        return _topicARN;
    }
}
