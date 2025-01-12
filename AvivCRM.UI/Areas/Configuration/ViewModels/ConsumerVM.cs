using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Configuraton.ViewModels;
public class ConsumerVM
{
    public Guid Id { get; set; }
	[Required(ErrorMessage = "Consumer Name should not be empty")]
    [MaxLength(10, ErrorMessage = "Consumer Name must not exceed 10 characters")]
    [MinLength(3, ErrorMessage = "Consumer Name should not be less than 3 characters")]
	public string? Code { get; set; }
    [Required(ErrorMessage = "Consumer Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Consumer Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Consumer Name should not be less than 3 characters")]
    public string Name { get; set; } = default!;
}






















