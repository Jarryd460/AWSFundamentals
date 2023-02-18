using Amazon.SimpleNotificationService.Model;

namespace Customers.Api.Messaging;

public interface ISnsMessager
{
    Task<PublishResponse> PublishMessageAsync<T>(T message);
}
