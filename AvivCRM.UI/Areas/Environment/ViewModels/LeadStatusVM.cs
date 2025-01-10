using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.Models;
public class LeadStatusVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Status Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Status Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Status Name should not be less than 3 characters")]
    public string? Name { get; set; } = default!;

    [Required(ErrorMessage = "color should not be empty")]
    public string? Color { get; set; } = default!;
}