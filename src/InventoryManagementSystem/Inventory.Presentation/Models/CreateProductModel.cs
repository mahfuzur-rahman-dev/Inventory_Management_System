using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class CreateProductModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
