namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class ProjectSettingVM
{
    public Guid Id { get; set; }
    public bool IsSendReminder { get; set; }
    public Guid ProjectReminderPersonId { get; set; }
    public int RemindBefore { get; set; }

    public ICollection<ProjectReminderPersonVM>? projectReminderPersons { get; set; }
}



