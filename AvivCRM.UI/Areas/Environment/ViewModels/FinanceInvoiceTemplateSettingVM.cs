namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class FinanceInvoiceTemplateSettingVM
{
    public Guid Id { get; set; }
    public string? FIRBTemplateJsonSettings { get; set; }
    public List<FIRBTemplateSettingVM>? FIRBTemplateSettings { get; set; }
}
public class FIRBTemplateSettingVM
{
    public int Id { get; set; }
    public string? ImageURL { get; set; }
    public bool isSelected { get; set; }
}



