using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class LeadCategoryVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Category Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Category Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Category Name should not be less than 3 characters")]
    public string? Name { get; set; } = default!;
}