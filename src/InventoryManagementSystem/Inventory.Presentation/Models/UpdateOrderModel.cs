using Inventory.DataAccess.Entites;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class UpdateOrderModel
    {
        [Required]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int SaleQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderType { get; set; }

        [ValidateNever]
        public Product Product { get; set; }

    }
}
