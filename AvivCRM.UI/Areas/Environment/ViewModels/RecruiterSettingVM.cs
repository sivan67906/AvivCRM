using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruiterSettingVM
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Recruiter Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Recruiter Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Recruiter Name should not be less than 3 characters")]
    public string? RecruiterName { get; set; }
    public Guid RecruiterStatusId { get; set; }
    public string? RecruiterStatusName { get; set; }
    public ToggleDDSettingVM? ToggleDDSettings { get; set; }
}