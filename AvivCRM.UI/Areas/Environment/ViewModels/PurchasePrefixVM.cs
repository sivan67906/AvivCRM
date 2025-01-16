namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class PurchaseVM
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
    public Guid Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}

public class PBillOrderVM
{
    public Guid Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}

public class PVendorCreditVM
{
    public Guid Id { get; set; }
    public string? Prefix { get; set; }
    public string? Seperator { get; set; }
    public int Digits { get; set; }
    public string? Example { get; set; }
}