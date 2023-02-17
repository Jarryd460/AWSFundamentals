namespace Customers.Api.Messaging;

internal sealed class Queue
{
    public const string Key = "Queue";
    public required string Name { get; init; }
}
