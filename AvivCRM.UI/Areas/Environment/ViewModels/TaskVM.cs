using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class TaskVM
{
    public Guid Id { get; set; }
	[Required(ErrorMessage = "Task Name should not be empty")]
    [MaxLength(10, ErrorMessage = "Task Name must not exceed 10 characters")]
    [MinLength(3, ErrorMessage = "Task Name should not be less than 3 characters")]
	public string? Code { get; set; }
    [Required(ErrorMessage = "Task Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Task Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Task Name should not be less than 3 characters")]
    public string Name { get; set; } = default!;
}






















