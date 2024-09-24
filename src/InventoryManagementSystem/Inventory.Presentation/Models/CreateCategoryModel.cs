using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class CreateCategoryModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
