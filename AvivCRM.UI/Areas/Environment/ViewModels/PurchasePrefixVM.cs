namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class PurchasePrefixVM
{
    public Guid Id { get; set; }
    public string? PurchasePrefixJsonSettings { get; set; }
    public CBPurchasePrefixVM? CBPurchasePrefixVM { get; set; }
}
public class CBPurchasePrefixVM
{
    public PPurchaseVM? PPurchaseVM { get; set; }
    public PBillOrderVM? PBillOrderVM { get; set; }
    public PVendorCreditVM? PVendorCreditVM { get; set; }
}
public class PPurchaseVM
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}
public class PBillOrderVM
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}
public class PVendorCreditVM
{
    public int Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}