using Inventory.DataAccess.Entites;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Inventory.Presentation.Models.VM
{
    public class ReportViewModel
    {
        public IEnumerable<Order> Reports { get; set; }
        public CreateReportModel CreateReportModel { get; set; }
        [ValidateNever]
        public string ReportType { get; set; }
    }
}
