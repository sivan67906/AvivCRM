namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class FinanceInvoiceSettingVM
{
    public Guid Id { get; set; }
    public IFormFile? FILogoImage { get; set; }
    public string? FILogoPath { get; set; }
    public string? FILogoImageFileName { get; set; }
    public IFormFile? FIAuthorisedImage { get; set; }
    public string? FIAuthorisedImagePath { get; set; }
    public string? FIAuthorisedImageFileName { get; set; }
    public Guid FILanguageId { get; set; }
    public int FIDueAfter { get; set; }
    public int FISendReminderBefore { get; set; }
    public int FISendReminderAfterEveryId { get; set; }
    public int FISendReminderAfterEvery { get; set; }
    public string? FICBGeneralJsonSettings { get; set; }
    public string? FICBClientInfoJsonSettings { get; set; }
    public string? FITerms { get; set; }
    public string? FIOtherInfo { get; set; }

    public LanguageDDSettingVM? LanguageDDSettings { get; set; }
    public List<FICBGeneralSettingVM>? FICBGeneralSettings { get; set; }
    public List<FICBClientInfoSettingVM>? FICBClientInfoSettings { get; set; }
}
public class FICBGeneralSettingVM
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsChecked { get; set; }
}
public class FICBClientInfoSettingVM
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsChecked { get; set; }
}
public class LanguageDDSettingVM
{
    public LanguageVM? language { get; set; }
    public Guid SelectedLanguageId { get; set; }
    public List<LanguageVM>? languageItems { get; set; }
}
public class LanguageVM
{
    public Guid Id { get; set; }
    public string? LanguageCode { get; set; }
    public string? LanguageName { get; set; }
}



