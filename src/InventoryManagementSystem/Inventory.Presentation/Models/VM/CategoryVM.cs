using Inventory.DataAccess.Entites;

namespace Inventory.Presentation.Models.VM
{
    public class CategoryVM
    {
        public CategoryVM()
        {
            UpdateCategoryData = new UpdateCategoryModel();
        }
        public UpdateCategoryModel UpdateCategoryData { get; set; }

        public Category Category { get; set; }
    }


}
