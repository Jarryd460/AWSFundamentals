using Amazon.SQS;
using Amazon.SQS.Model;

var cts = new CancellationTokenSource();
var sqsClient = new AmazonSQSClient();


var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers").ConfigureAwait(false);

var receiveMessageRequest = new ReceiveMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    // To be effient, queues do not return attributes so you have to specify what attributes to return
    AttributeNames = new List<string>() { "ApproximateReceiveCount" },
    MessageAttributeNames = new List<string>() { "All" }
};

while (!cts.IsCancellationRequested)
{
    // Read message from queue
    var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token).ConfigureAwait(false);

    foreach (var message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        // Delete message from queue as it's not automatically deleted after reading
        // If message is not deleted, message will be read again 30 seconds later
        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, cts.Token).ConfigureAwait(false);
    }

    await Task.Delay(3000).ConfigureAwait(false);
}