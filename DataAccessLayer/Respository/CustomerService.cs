using DataAccessLayer.Model;
using DataAccessLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DataAccessLayer.Respository
{
    public class CustomerService : ICustomerInterface
    {
        private IMongoCollection<Customer> customerCollection;

        public CustomerService(string connectionSring, string databaseName)
        {
            var mongoClient = new MongoClient(connectionSring);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            customerCollection = mongoDatabase.GetCollection<Customer>("Customer");
        }

        public List<Customer> CustomerListAsync()
        {
            return  customerCollection.Find(customer => true).ToList();
        }

        public Customer GetCustomerById(string id)
        {
            var objcustomer = customerCollection.Find(i => i.Id == id).FirstOrDefault();
            return objcustomer;
        }

        public async Task AddCustomer(Customer customer)
        {
            await customerCollection.InsertOneAsync(customer);
        }

        public async Task UpdateCustomer(string id, Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
            var update = Builders<Customer>.Update
                .Set(c => c.CustomerName, customer.CustomerName)
                .Set(c => c.Email, customer.Email)
                .Set(c => c.Mobile, customer.Mobile);
            await customerCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteCustomer(string id)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, id);
            await customerCollection.DeleteOneAsync(filter);
        }
    }
}
