using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class ProjectStatusVM
{
    [Required(ErrorMessage = "Project Status Id should not be empty")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Project Status Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Project Status Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Project Status Name should not be less than 3 characters")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Project select the color")]
    public string? ColorCode { get; set; }

    public bool IsDefaultStatus { get; set; }
    public bool Status { get; set; }
}