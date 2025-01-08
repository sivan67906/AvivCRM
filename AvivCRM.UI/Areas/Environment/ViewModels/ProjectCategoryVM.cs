using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class ProjectCategoryVM
{
    [Required] public Guid Id { get; set; }

    [Required(ErrorMessage = "Project Category Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Project Category Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Project Category Name should not be less than 3 characters")]
    public string? Name { get; set; }
}