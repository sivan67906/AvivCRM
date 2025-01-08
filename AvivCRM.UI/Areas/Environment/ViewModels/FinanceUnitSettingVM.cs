using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class FinanceUnitSettingVM
{
    public Guid Id { get; set; }
    public string? FUnitCode { get; set; }

    [Required(ErrorMessage = "Finance Unit Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Finance Unit Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Finance Unit Name should not be less than 3 characters")]
    public string? FUnitName { get; set; }

    public bool FIsDefault { get; set; }
}