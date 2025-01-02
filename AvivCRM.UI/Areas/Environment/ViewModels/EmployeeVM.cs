namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class EmployeeVM
{
    public int Id { get; set; }
    public int EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Description { get; set; }
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int AddressId { get; set; }
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public string StateName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
}



