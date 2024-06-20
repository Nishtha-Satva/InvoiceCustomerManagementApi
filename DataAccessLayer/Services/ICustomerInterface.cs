using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public interface ICustomerInterface
    {
        Task AddCustomer(Customer customer);

        List<Customer> CustomerListAsync();

        Task UpdateCustomer(string id, Customer customer);

        Task DeleteCustomer(string id);
        List<Invoice> GetInvoicesByCustomerId(string customerId);

        Task<Customer> FindCustomerByEmail(string email);

        Task<Customer> FindCustomerById(string id);
    }
}
