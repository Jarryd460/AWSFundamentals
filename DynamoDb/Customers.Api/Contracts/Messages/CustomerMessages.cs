namespace Customers.Api.Contracts.Messages;

internal sealed class CustomerCreated
{
    public required Guid Id { get; init; }
    public required string Fullname { get; init; }
    public required string Email { get; init; }
    public required string GitHubUsername { get; init; }
    public required DateTime DateOfBirth { get; init; }
}

internal sealed class CustomerUpdated
{
    public required Guid Id { get; init; }
    public required string Fullname { get; init; }
    public required string Email { get; init; }
    public required string GitHubUsername { get; init; }
    public required DateTime DateOfBirth { get; init; }
}

internal sealed class CustomerDelete
{
    public required Guid Id { get; init; }
}
