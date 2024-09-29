using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Entites
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Product>? Products { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Order>? Orders { get; set; }
    }
}

