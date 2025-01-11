namespace AvivCRM.UI.Areas.Environment.ViewModels;

//public class ToggleValueVM
//{
//    public Guid Id { get; set; }
//    public string? TCode { get; set; }
//    public bool TValue { get; set; }
//}
public class ToggleDDSettingVM
{
    public ToggleValueVM? ToggleValueVM { get; set; }
    public Guid SelectedToggleValueId { get; set; }
    public List<ToggleValueVM>? toggleValues { get; set; }
}