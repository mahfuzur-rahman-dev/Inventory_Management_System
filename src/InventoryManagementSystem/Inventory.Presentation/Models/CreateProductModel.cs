using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class CreateProductModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal MinimumSellingPrice { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal BuyingPrice { get; set; }

        [Required(ErrorMessage = "Product quantity is required")]
        public int QuantityInStock { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

    }
}
