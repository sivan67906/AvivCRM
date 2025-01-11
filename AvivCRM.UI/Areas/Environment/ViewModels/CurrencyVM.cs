using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class CurrencyVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = " CurrencyName should not be empty")]
    [MaxLength(25, ErrorMessage = " CurrencyName must not exceed 25 characters")]
    [MinLength(2, ErrorMessage = " CurrencyName should not be less than 2 characters")]
    public string? CurrencyName { get; set; } = default!;

    [Required(ErrorMessage = " CurrencySymbol should not be empty")]
    [MaxLength(5, ErrorMessage = " CurrencySymbol must not exceed 25 characters")]
    [MinLength(1, ErrorMessage = " CurrencySymbol should not be less than 1 characters")]
    public string? CurrencySymbol { get; set; } = default!;

    [Required(ErrorMessage = " CurrencyCode should not be empty")]
    [MaxLength(5, ErrorMessage = " CurrencyCode must not exceed 25 characters")]
    [MinLength(1, ErrorMessage = " CurrencyCode should not be less than 1 characters")]
    public string? CurrencyCode { get; set; } = default!;
    public string? IsCryptocurrency { get; set; }
    public int USDPrice { get; set; }
    public int ExchangeRate { get; set; }
    public string? CurrencyPosition { get; set; }
    public string? ThousandSeparator { get; set; }
    public string? DecimalSeparator { get; set; }
    public int NumberofDecimals { get; set; }
}