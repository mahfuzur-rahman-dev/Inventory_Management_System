using Inventory.DataAccess.Entites;

namespace Inventory.Presentation.Models
{
    public class CreateSaleOrderModel
    {
        public Guid ProductId { get; set; }
        public int SaleQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
