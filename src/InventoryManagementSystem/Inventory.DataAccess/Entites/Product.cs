using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Entites
{
    public class Product
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal BuyingPrice { get; set; } 
        public decimal MinimumSellingPrice { get; set; } 
        public int QuantityInStock { get; set; }
        public Guid CategoryId { get; set; }


        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
