namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class ProjectSettingVM
{
    public int Id { get; set; }
    public bool IsSendReminder { get; set; }
    public int ProjectReminderPersonId { get; set; }
    public int RemindBefore { get; set; }

    public ICollection<ProjectReminderPersonVM>? projectReminderPersons { get; set; }
}



