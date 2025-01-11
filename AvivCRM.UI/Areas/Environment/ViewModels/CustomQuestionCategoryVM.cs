using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class CustomQuestionCategoryVM
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Custom Question Category Name should not be empty")]
    [MaxLength(10, ErrorMessage = "Custom Question Category Name must not exceed 10 characters")]
    [MinLength(3, ErrorMessage = "Custom Question Category Name should not be less than 3 characters")]
    public string? CQCategoryCode { get; set; }
    [Required(ErrorMessage = "Custom Question Category Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Custom Question Category Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Custom Question Category Name should not be less than 3 characters")]
    public string CQCategoryName { get; set; } = default!;
}






















