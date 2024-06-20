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
    public class Items
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ItemCode { get; set; }
        [Required(ErrorMessage = "Item name is required.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Unit price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
        public double UnitPrice { get; set; }

    }

    public class ItemInvoice : Items
    {
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Qty must be greater than 0.")]
        public int Qty { get; set; }

        public double SubTotal { get; set; }
    }

}

