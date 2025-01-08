namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitGeneralSettingVM
{
    public int Id { get; set; }
    public string? GeneralCompanyName { get; set; }
    public string? GeneralCompanyWebsite { get; set; }
    public string? GeneralCompanyLogo { get; set; }
    public IFormFile? GeneralCompanyLogoImage { get; set; }
    public string? GeneralCompanyLogoPath { get; set; }
    public string? GeneralCompanyLogoImageFileName { get; set; }
    public string? GeneralAboutCompany { get; set; }
    public string? GeneralLegalTerm { get; set; }
    public int GeneralDuplJobApplnRestrictDays { get; set; }
    public int GeneralOLReminderToCandidate { get; set; }
    public string? GeneralBGLogo { get; set; }
    public IFormFile? GeneralBGLogoImage { get; set; }
    public string? GeneralBGLogoPath { get; set; }
    public string? GeneralBGLogoImageFileName { get; set; }

    public string? GeneralBGColorCode { get; set; }

    //public string? GeneralRBJsonSettings { get; set; }
    public string? GeneralCBJsonSettings { get; set; }

    public List<GeneralCBSettingVM>? GeneralCBSettings { get; set; }
    //public List<GeneralRBSettingVM>? GeneralRBSettings { get; set; }
}