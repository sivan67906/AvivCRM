using AvivCRM.UI.Areas.Configuration.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class TimeLogVM
{
    public int Id { get; set; }
    public string? CBTimeLogJsonSettings { get; set; }
    public bool IsTimeTrackerReminderEnabled { get; set; }
    public string? TLTime { get; set; }
    public bool IsDailyTimeLogReportEnabled { get; set; }
    public int RoleId { get; set; }
    //public string? RoleName { get; set; }
    public RoleDDSetting? RoleDDSettings { get; set; }
    public List<CBTimeLogSettingVM>? CBTimeLogSettings { get; set; }
}

public class RoleDDSetting
{
    public RoleVM? role { get; set; }
    public int SelectedRoleId { get; set; }
    public List<RoleVM>? roleItems { get; set; }
}



