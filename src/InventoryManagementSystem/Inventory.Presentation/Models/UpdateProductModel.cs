using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class UpdateProductModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product quantity is required")]
        public int QuantityInStock { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

    }
}
