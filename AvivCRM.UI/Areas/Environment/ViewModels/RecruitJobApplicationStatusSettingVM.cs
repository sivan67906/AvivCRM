using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitJobApplicationStatusSettingVM
{
    public Guid Id { get; set; }
    public Guid JobApplicationPositionId { get; set; }
    public string? JobApplicationPositionName { get; set; }
    public Guid JobApplicationCategoryId { get; set; }
    public string? JobApplicationCategoryName { get; set; }
    [Required(ErrorMessage = "Status should not be empty")]
    [MaxLength(100, ErrorMessage = "Status must not exceed 100 characters")]
    [MinLength(3, ErrorMessage = "Status should not be less than 3 characters")]
    public string? JASStatus { get; set; }
    public string? JASColor { get; set; }
    public bool JASIsModelChecked { get; set; }

    public JobApplicationCategoryDDSettingVM? JobApplicationCategoryDDSetting { get; set; }
    public JobApplicationPositionDDSettingVM? JobApplicationPositionDDSetting { get; set; }
}

public class JobApplicationCategoryDDSettingVM
{
    public JobApplicationCategoryVM? JobApplicationCategory { get; set; }
    public Guid SelectedId { get; set; }
    public List<JobApplicationCategoryVM>? JobApplicationCategoryList { get; set; }
}

public class JobApplicationPositionDDSettingVM
{
    public JobApplicationPositionVM? JobApplicationPosition { get; set; }
    public Guid SelectedId { get; set; }
    public List<JobApplicationPositionVM>? JobApplicationPositionList { get; set; }
}