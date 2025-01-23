namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class ProjectVM
{
    public ProjectSettingVM? ProjectSetting { get; set; }
    public List<ProjectStatusVM>? ProjectStatuses { get; set; }
    public List<ProjectCategoryVM>? ProjectCategories { get; set; }

    public Guid Id { get; set; }
    public string? ProjectName { get; set; }
}