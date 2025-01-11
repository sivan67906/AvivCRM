using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class PlanningVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = " Name should not be empty")]
    [MaxLength(25, ErrorMessage = " Name must not exceed 25 characters")]
    [MinLength(1, ErrorMessage = " Name should not be less than 3 characters")]
    public string? Name { get; set; } = default!;

    [Required(ErrorMessage = "Plan Price should not be empty")]
    [Range(1, 100000, ErrorMessage = "Please give proper Plan Price ")]
    public float PlanPrice { get; set; } = default!;

    [Required(ErrorMessage = "Validity should not be empty")]
    [Range(0, 24, ErrorMessage = "Validity Required 24 or below month")]
    public int Validity { get; set; } = default!;

    [Required(ErrorMessage = "Employee should not be empty")]
    [Range(0, 50, ErrorMessage = " Employee must not exceed 50 ")]
    public int Employee { get; set; } = default!;

    [Required(ErrorMessage = "Designation should not be empty")]
    [Range(0, 5, ErrorMessage = "Designation  must not exceed 5")]
    public int Designation { get; set; } = default!;

    [Required(ErrorMessage = "Department should not be empty")]
    [Range(0, 5, ErrorMessage = "Department  must not exceed 5")]
    public int Department { get; set; } = default!;

    [Required(ErrorMessage = "Company should not be empty")]
    [Range(0, 2, ErrorMessage = "Company must not exceed 2")]
    public int Company { get; set; } = default!;

    [Required(ErrorMessage = "Roles should not be empty")]
    [Range(0, 15, ErrorMessage = "Roles must not exceed 15")]
    public int Roles { get; set; } = default!;

    [Required(ErrorMessage = "Permission should not be empty")]
    [Range(0, 20, ErrorMessage = "Permission must not exceed 20")]
    public int Permission { get; set; } = default!;
    public string? Description { get; set; }
}