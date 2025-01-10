using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class PaymentVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = " Method should not be empty")]
    [MaxLength(25, ErrorMessage = " Method must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = " Method should not be less than 3 characters")]
    public string? Method { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public bool Status { get; set; } = true;
}