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
        public decimal Price { get; set; } = 0.00m;
        public int QuantityInStock { get; set; } = 0;
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } 

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
