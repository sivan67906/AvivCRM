namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class BillOrderVM
{
    public Guid Id { get; set; }
    public string? BillOrderPrefix { get; set; }
    public string? BillOrderNumberSeperater { get; set; }
    public string? BillOrderNumberDigits { get; set; }
    public string? BillOrderNumberExample { get; set; }
}