namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class ContractVM
{
    public Guid Id { get; set; }
    public string? ContractPrefix { get; set; }
    public string? ContractNumberSeprator { get; set; }
    public int ContractNumberDigits { get; set; }
    public string? ContractNumberExample { get; set; }
}