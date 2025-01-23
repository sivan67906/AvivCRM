namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class TimesheetSettingVM
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public Guid TaskId { get; set; }
    public string? TaskName { get; set; }
    public Guid EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime? StartDate { get; set; }
    public string? StartTime { get; set; }
    public string? StartDateTime { get; set; }
    public DateTime? EndDate { get; set; }
    public string? EndTime { get; set; }
    public string? EndDateTime { get; set; }
    public string? Memo { get; set; }
    public int TotalHours { get; set; }
}

public class TaskingVM
{
    public Guid Id { get; set; }
    public string? TaskName { get; set; }
}