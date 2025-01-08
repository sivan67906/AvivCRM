namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class FinanceVM
{
    public FinanceInvoiceSettingVM? FinanceInvoiceSettingVMList { get; set; }
    public FinanceInvoiceTemplateSettingVM? FinanceInvoiceTemplateSettingVMList { get; set; }
    public FinancePrefixSettingVM? FinancePrefixSettingVMList { get; set; }
    public List<FinanceUnitSettingVM>? FinanceUnitSettingVMList { get; set; }
}