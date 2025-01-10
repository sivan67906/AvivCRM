namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitJobApplicationStatusSettingVM
{
    public Guid Id { get; set; }
    public Guid JobApplicationPositionId { get; set; }
    public string? JobApplicationPositionName { get; set; }
    public Guid JobApplicationCategoryId { get; set; }
    public string? JobApplicationCategoryName { get; set; }
    public string? JASStatus { get; set; }
    public string? JASColor { get; set; }
    public bool JASIsModelChecked { get; set; }
}