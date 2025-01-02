namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class StateVM
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public required string Name { get; set; }
    public int CountryId { get; set; }
    public string? CountryName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}



