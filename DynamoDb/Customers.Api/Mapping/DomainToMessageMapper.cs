using Customers.Api.Contracts.Messages;
using Customers.Api.Domain;

namespace Customers.Api.Mapping
{
    internal static class DomainToMessageMapper
    {
        public static CustomerCreated ToCustomerCreatedMessage(this Customer customer)
        {
            return new CustomerCreated()
            {
                Id = customer.Id,
                Fullname = customer.FullName,
                Email = customer.Email,
                GitHubUsername = customer.GitHubUsername,
                DateOfBirth = customer.DateOfBirth
            };
        }

        public static CustomerUpdated ToCustomerUpdatedMessage(this Customer customer)
        {
            return new CustomerUpdated()
            {
                Id = customer.Id,
                Fullname = customer.FullName,
                Email = customer.Email,
                GitHubUsername = customer.GitHubUsername,
                DateOfBirth = customer.DateOfBirth
            };
        }
    }
}
