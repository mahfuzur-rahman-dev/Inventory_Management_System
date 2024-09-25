using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class UpdateProductModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityInStock { get; set; }

        public Guid? CategoryId { get; set; }



    }
}
