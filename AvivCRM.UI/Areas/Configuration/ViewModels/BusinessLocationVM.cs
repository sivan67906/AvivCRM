namespace AvivCRM.UI.Areas.Configuration.ViewModels;
public class BusinessLocationVM
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public int AddressId { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? ZipCode { get; set; }
    public string? TaxName { get; set; }
    public string? TaxNumber { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public int CountryId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public string? CountryName { get; set; }
    public string? StateName { get; set; }
    public string? CityName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}