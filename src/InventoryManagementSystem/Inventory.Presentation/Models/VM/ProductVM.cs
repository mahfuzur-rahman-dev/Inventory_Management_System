using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Presentation.Models.VM
{
    public class ProductVM
    {
        public CreateProductModel Product { get; set; }
        public ProductVM()
        {
            Product = new CreateProductModel();
        }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryNames { get; set; }
    }
}
