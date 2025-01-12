using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class JobApplicationPositionVM
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Job Application Position should not be empty")]
    [MaxLength(10, ErrorMessage = "Job Application Position must not exceed 10 characters")]
    [MinLength(3, ErrorMessage = "Job Application Position should not be less than 3 characters")]
    public string? JAPositionCode { get; set; }
    [Required(ErrorMessage = "Job Application Position should not be empty")]
    [MaxLength(25, ErrorMessage = "Job Application Position must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Job Application Position should not be less than 3 characters")]
    public string JAPositionName { get; set; } = default!;
}






















