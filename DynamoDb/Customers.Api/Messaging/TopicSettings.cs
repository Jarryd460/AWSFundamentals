namespace Customers.Api.Messaging;

internal sealed class TopicSettings
{
    public const string Key = "Topic";
    public required string Name { get; init; }
}
