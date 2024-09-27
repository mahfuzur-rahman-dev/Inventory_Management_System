using Inventory.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Entites
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? SuplierId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; } 
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
