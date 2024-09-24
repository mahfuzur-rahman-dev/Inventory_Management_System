using Inventory.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProduct()
        {
             var products = await _unitOfWork.Products.GetAllAsync();
            return View(products);
        }
    }
}
