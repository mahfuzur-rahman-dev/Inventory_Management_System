using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class CreateReportModel
    {
        public string SearchFrom { get; set; }
        public string SearchTo { get; set; }

    }
}
