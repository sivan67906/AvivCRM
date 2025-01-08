namespace AvivCRM.UI.Areas.Configuration.ViewModels;
public class BusinessCategoryVM
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int BusinessTypeId { get; set; }
    public string? BusinessTypeName { get; set; }
}