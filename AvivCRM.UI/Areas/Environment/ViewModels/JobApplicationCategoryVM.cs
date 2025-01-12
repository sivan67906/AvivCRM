using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class JobApplicationCategoryVM
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Job Application Category Name should not be empty")]
    [MaxLength(10, ErrorMessage = "Job Application Category Name must not exceed 10 characters")]
    [MinLength(3, ErrorMessage = "Job Application Category Name should not be less than 3 characters")]
    public string? JACategoryCode { get; set; }
    [Required(ErrorMessage = "Job Application Category Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Job Application Category Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Job Application Category Name should not be less than 3 characters")]
    public string JACategoryName { get; set; } = default!;
}






















