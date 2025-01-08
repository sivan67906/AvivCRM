namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitNotificationSettingVM
{
    public int Id { get; set; }
    public string? CBEMailJsonSettings { get; set; }
    public string? CBEMailNotificationJsonSettings { get; set; }
    public List<CBEMailSettingVM>? CBEMailSettings { get; set; }
    public List<CBEMailNotificationSettingVM>? CBEMailNotificationSettings { get; set; }
}