using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class LeadAgentVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Agent Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Agent Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Agent Name should not be less than 3 characters")]
    public string? Name { get; set; } = default!;
}