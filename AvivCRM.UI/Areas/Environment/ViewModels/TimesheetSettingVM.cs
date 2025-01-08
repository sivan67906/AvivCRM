namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class TimesheetSettingVM
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public int TaskId { get; set; }
    public string? TaskName { get; set; }
    public int EmployeeId { get; set; }
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