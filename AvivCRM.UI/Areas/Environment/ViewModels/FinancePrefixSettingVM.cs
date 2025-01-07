namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class FinancePrefixSettingVM
{
    public Guid Id { get; set; }
    public string? FICBPrefixJsonSettings { get; set; }
    //public string? FICBPrefixInvoiceJsonSettings { get; set; }
    //public string? FICBPrefixOrderJsonSettings { get; set; }
    //public string? FICBPrefixCreditNoteJsonSettings { get; set; }
    //public string? FICBPrefixEstimationJsonSettings { get; set; }
    public FICBPrefixSettingVM? FICBPrefixSettingVM { get; set; }
}
public class FICBPrefixSettingVM
{
    public FPInvoiceVM? FPInvoiceVM { get; set; }
    public FPCreditNoteVM? FPCreditNoteVM { get; set; }
    public FPEstimationVM? FPEstimationVM { get; set; }
    public FPOrderVM? FPOrderVM { get; set; }
}
public class FPInvoiceVM
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}
public class FPCreditNoteVM
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}
public class FPEstimationVM
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}
public class FPOrderVM
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}