using System.Text.Json.Serialization;

namespace Customers.Api.Contracts.Data;

public class CustomerDto
{
    // to match property in json
    [JsonPropertyName("pk")]
    public string Pk => Id.ToString();

    // to match property in json
    [JsonPropertyName("sk")]
    public string Sk => Id.ToString();

    public Guid Id { get; init; } = default!;

    public string GitHubUsername { get; init; } = default!;

    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public DateTime DateOfBirth { get; init; }

    public DateTime UpdatedAt { get; set; }
}
