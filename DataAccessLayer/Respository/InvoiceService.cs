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
        private readonly IMongoCollection<Invoice> invoiceCollection;
        private readonly IMongoCollection<Items> itemCollection;

        public InvoiceService(string connectionSring, string databaseName)
        {
            var mongoClient = new MongoClient(connectionSring);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            invoiceCollection = mongoDatabase.GetCollection<Invoice>("Invoice");
            itemCollection = mongoDatabase.GetCollection<Items>("Items");
        }
        public List<Invoice> InvoiceListAsync()
        {
            return invoiceCollection.Find(customer => true).ToList();
        }  
        public Invoice GetInvoiceById(string id)
        {
            var objInvoice = invoiceCollection.Find(i => i.Id == id).FirstOrDefault();
            return objInvoice;
        }
        public async Task DeleteInvoiceAsync(string id)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.Id, id);
            await invoiceCollection.DeleteOneAsync(filter);
        }
        public async Task AddInvoice(Invoice invoice)
        {
            invoice.LineItems.ForEach(p => p.SubTotal = p.Qty * p.UnitPrice);
            double total = invoice.LineItems.Sum(item => item.UnitPrice * item.Qty);
            invoice.Total = total;
            await invoiceCollection.InsertOneAsync(invoice);
        }
        public async Task<Invoice> FindInvoiceNumberAsync(string invoiceNumber)
        {
            return await invoiceCollection.Find(i => i.InvoiceNumber == invoiceNumber).FirstOrDefaultAsync();
        }
        public async Task UpdateInvoiceAsync(string id, Invoice invoice)
        {
            invoice.LineItems.ForEach(p => p.SubTotal = p.Qty * p.UnitPrice);
            double total = invoice.LineItems.Sum(item => item.UnitPrice * item.Qty);
            invoice.Total = total;
            var filter = Builders<Invoice>.Filter.Eq(i => i.Id, id);
            var update = Builders<Invoice>.Update
                .Set(i => i.InvoiceDate, invoice.InvoiceDate)
                .Set(i => i.InvoiceNumber, invoice.InvoiceNumber)
                .Set(i => i.BillingAddress, invoice.BillingAddress)
                .Set(i => i.ShippingAddress, invoice.ShippingAddress)
                .Set(i => i.DueDate, invoice.DueDate)
                .Set(i => i.Status, invoice.Status)
                .Set(i => i.LineItems, invoice.LineItems)
                .Set(i => i.Total, invoice.Total);
            await invoiceCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateStatusAsnyc(string id, Invoice invoice)
        {
            var filter = Builders<Invoice>.Filter.Eq(i => i.Id, id);
            var update = Builders<Invoice>.Update
                .Set(i => i.Status, invoice.Status);
            await invoiceCollection.UpdateOneAsync(filter, update);
        }

    }
}
