namespace Customers.Consumer.Messages;

public sealed class CustomerCreated : ISqsMessage
{
    public required Guid Id { get; init; }
    public required string Fullname { get; init; }
    public required string Email { get; init; }
    public required string GitHubUsername { get; init; }
    public required DateTime DateOfBirth { get; init; }
}

public sealed class CustomerUpdated : ISqsMessage
{
    public required Guid Id { get; init; }
    public required string Fullname { get; init; }
    public required string Email { get; init; }
    public required string GitHubUsername { get; init; }
    public required DateTime DateOfBirth { get; init; }
}

public sealed class CustomerDelete : ISqsMessage
{
    public required Guid Id { get; init; }
}
