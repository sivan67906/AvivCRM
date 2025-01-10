using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class TaxVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Tax Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Tax Name must not exceed 25 characters")]
    [MinLength(1, ErrorMessage = "Tax Name should not be less than 3 characters")]
    public string? Name { get; set; } = default!;

    [Required(ErrorMessage = "Rate should not be empty")]
    [Range(0, 100, ErrorMessage = "Rate Required 1 to 100 in percentage")]
    public float Rate { get; set; } = default!;
}