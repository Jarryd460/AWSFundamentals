using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Customers.Api.Contracts.Data;
using System.Net;
using System.Text.Json;

namespace Customers.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDB;
    private readonly string _tableItem = "customers";

    public CustomerRepository(IAmazonDynamoDB dynamoDB)
    {
        _dynamoDB = dynamoDB;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        // Create a request object
        var createItemRequest = new PutItemRequest()
        {
            TableName = _tableItem,
            Item = customerAsAttributes,
            // ensures that the pk does not exist.
            // if this condition is not added then the record will be inserted or updated depending on if the pk exists. You could also add sk and in that case the combination of both the pk and sk needs to be unique
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)"
        };

        // pass request object to dynamo db to make request
        var response = await _dynamoDB.PutItemAsync(createItemRequest).ConfigureAwait(false);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest()
        {
            TableName = _tableItem,
            Key = new Dictionary<string, AttributeValue>()
            {
                {
                    "pk",
                    new AttributeValue()
                    {
                        S = id.ToString()
                    }
                },
                {
                    "sk",
                    new AttributeValue()
                    {
                        S = id.ToString()
                    }
                }
            }
        };

        var response = await _dynamoDB.GetItemAsync(getItemRequest).ConfigureAwait(false);

        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<CustomerDto>(itemAsDocument.ToJson());
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        // Never use scanning as it slows the application as data grows
        var scanRequest = new ScanRequest()
        {
            TableName = _tableItem
        };

        var response = await _dynamoDB.ScanAsync(scanRequest).ConfigureAwait(false);

        return response.Items.Select(x =>
        {
            var json = Document.FromAttributeMap(x).ToJson();
            return JsonSerializer.Deserialize<CustomerDto>(json);
        })!;
    }

    public async Task<bool> UpdateAsync(CustomerDto customer, DateTime requestStarted)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAsAttributes = Document.FromJson(customerAsJson).ToAttributeMap();

        // Create a request object
        var updateItemRequest = new PutItemRequest()
        {
            TableName = _tableItem,
            Item = customerAsAttributes,
            ConditionExpression = "UpdatedAt < :requestStarted",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                {
                    ":requestStarted",
                    new AttributeValue()
                    {
                        S = requestStarted.ToString("O")
                    }
                }
            }
        };

        // pass request object to dynamo db to make request
        var response = await _dynamoDB.PutItemAsync(updateItemRequest).ConfigureAwait(false);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest()
        {
            TableName = _tableItem,
            Key = new Dictionary<string, AttributeValue>()
            {
                {
                    "pk",
                    new AttributeValue()
                    {
                        S = id.ToString()
                    }
                },
                {
                    "sk",
                    new AttributeValue()
                    {
                        S = id.ToString()
                    }
                }
            }
        };

        var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest).ConfigureAwait(false);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}
