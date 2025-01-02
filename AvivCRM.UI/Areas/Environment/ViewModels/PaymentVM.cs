namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class PaymentVM
{
    public int Id { get; set; }
    public string? Method { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; } = true;
}



