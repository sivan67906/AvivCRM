namespace AvivCRM.UI.Areas.Configuration.ViewModels;
public class CityVM
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public required string Name { get; set; }
    public int StateId { get; set; }
    public string? StateName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}
