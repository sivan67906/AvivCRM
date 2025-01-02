using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.Models
{
    public class LeadSourceVM
    {
        [Key]
        public int Id { get; set; }
        public string? source { get; set; }
    }
}



