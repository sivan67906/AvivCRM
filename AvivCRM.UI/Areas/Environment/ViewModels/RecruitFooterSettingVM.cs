using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitFooterSettingVM
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Title should not be empty")]
    [MaxLength(25, ErrorMessage = "Title must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Title should not be less than 3 characters")]
    public string? FooterTitle { get; set; }
    [Required(ErrorMessage = "Slug should not be empty")]
    [MaxLength(25, ErrorMessage = "Slug must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Slug should not be less than 3 characters")]
    public string? FooterSlug { get; set; }
    public Guid FooterStatusId { get; set; }
    public string? FooterStatusName { get; set; }
    [Required(ErrorMessage = "Description should not be empty")]
    [MaxLength(250, ErrorMessage = "Description must not exceed 250 characters")]
    [MinLength(3, ErrorMessage = "Description should not be less than 3 characters")]
    public string? FooterDescription { get; set; }
    public ToggleDDSettingVM? ToggleDDSettings { get; set; }
}
