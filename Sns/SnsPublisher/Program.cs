using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;
using System.Text.Json;

var customer = new CustomerCreated()
{
    Id = Guid.NewGuid(),
    Fullname = "Jarryd Deane",
    Email = "jarryd.deane@gmail.com",
    GitHubUsername = "Jarryd460",
    DateOfBirth = new DateTime(1992, 4, 29)
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicArn = await snsClient.FindTopicAsync("customers").ConfigureAwait(false);

var publishRequest = new PublishRequest()
{
    TopicArn = topicArn.TopicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
    {
        {
            "MessageType",
            new MessageAttributeValue()
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var response = await snsClient.PublishAsync(publishRequest).ConfigureAwait(false);