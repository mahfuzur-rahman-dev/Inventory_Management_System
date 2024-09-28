using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentation.Models
{
    public class CreateReportModel
    {
        [Required]
        public DateTime SearchFrom { get; set; }
        [Required]
        public DateTime SearchTo { get; set; }

        
    }
}
