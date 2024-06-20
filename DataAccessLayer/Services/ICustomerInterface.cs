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

        Customer GetCustomerById(string id);

        Task UpdateCustomer(string id, Customer customer);

        Task DeleteCustomer(string id);
    }
}
