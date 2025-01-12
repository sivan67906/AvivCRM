using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class LanguageVM
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Language Name should not be empty")]
    [MaxLength(10, ErrorMessage = "Language Name must not exceed 10 characters")]
    [MinLength(3, ErrorMessage = "Language Name should not be less than 3 characters")]
    public string? LanguageCode { get; set; }
    [Required(ErrorMessage = "Language Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Language Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Language Name should not be less than 3 characters")]
    public string LanguageName { get; set; } = default!;
}






















