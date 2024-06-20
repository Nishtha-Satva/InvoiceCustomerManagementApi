using DataAccessLayer.Model;
using DataAccessLayer.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository
{
    public class InvoiceService : IInvoiceInterface
    {
        private IMongoCollection<Invoice> customerCollection;

        public InvoiceService(string connectionSring, string databaseName)
        {
            var mongoClient = new MongoClient(connectionSring);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            customerCollection = mongoDatabase.GetCollection<Invoice>("Invoice");
        }

    }
}
