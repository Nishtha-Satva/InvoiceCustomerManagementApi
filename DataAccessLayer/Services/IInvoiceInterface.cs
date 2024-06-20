using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public interface IInvoiceInterface
    {
        List<Invoice> InvoiceListAsync();

        Invoice GetInvoiceById(string id);

        Task DeleteInvoiceAsync(string id);

        Task AddInvoice(Invoice invoice);

        Task UpdateInvoiceAsync(string id, Invoice invoice);

        Task<Invoice> FindInvoiceNumberAsync(string invoiceNumber);

        Task UpdateStatusAsnyc(string id, Invoice invoice);
    }
}
