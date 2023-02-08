using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;
using System.Text.Json;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated()
{
    Id = Guid.NewGuid(),
    Fullname = "Jarryd Deane",
    Email = "jarryd.deane@gmail.com",
    GitHubUsername = "Jarryd460",
    DateOfBirth = new DateTime(1992, 4, 29)
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers").ConfigureAwait(false);

var sendMessageRequest = new SendMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
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

// Send a message to the queue
var response = await sqsClient.SendMessageAsync(sendMessageRequest).ConfigureAwait(false);

Console.WriteLine();