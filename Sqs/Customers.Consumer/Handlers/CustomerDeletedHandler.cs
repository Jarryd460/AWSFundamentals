using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers
{
    public class CustomerDeletedHandler : IRequestHandler<CustomerDelete>
    {
        private readonly ILogger<CustomerDeletedHandler> _logger;

        public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(CustomerDelete request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.Id.ToString());
            return Unit.Task;
        }
    }
}
