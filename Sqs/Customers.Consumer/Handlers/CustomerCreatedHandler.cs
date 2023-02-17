using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers
{
    internal sealed class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedHandler> _logger;

        public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(CustomerCreated request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.Fullname);
            return Unit.Task;
        }
    }
}
