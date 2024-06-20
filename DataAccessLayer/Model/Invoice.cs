using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class Invoice
    {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string? Id { get; set; }

            [Required(ErrorMessage = "Invoice number is required.")]
            public string InvoiceNumber { get; set; }

            [Required(ErrorMessage = "Invoice Date is required.")]
            public DateTime InvoiceDate { get; set; }

            [Required(ErrorMessage = "Due date is required.")]
            public DateTime DueDate { get; set; }

            [Required(ErrorMessage = "Billing address is required.")]
            public Address BillingAddress { get; set; }

            [Required(ErrorMessage = "Shipping address is required.")]
            public Address ShippingAddress { get; set; }

            public string CustomerId { get; set; }

            [Range(1, 2, ErrorMessage = "Invalid status type")]
            public Status Status { get; set; }

            [Required(ErrorMessage = "At least one line item is required.")]

            public List<ItemInvoice> LineItems { get; set; }

            public double? Total { get; set; }

        }

        public class Address
        {

            [Required(ErrorMessage = "Street address is required.")]
            public string StreetAddress { get; set; }

            [Required(ErrorMessage = "City is required.")]
            public string City { get; set; }

            [Required(ErrorMessage = "Postal code is required.")]
            public string PostalCode { get; set; }

            [Required(ErrorMessage = "Country is required.")]
            public string Country { get; set; }

            [Required(ErrorMessage = "State is required.")]
            public string State { get; set; }

        }

    public enum Status
    {
        Paid = 1,
        Unpaid = 2,
    }
}

