namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitVM
{
    public RecruitGeneralSettingVM? RecruitGeneralSettingVMList { get; set; }
    public List<RecruitFooterSettingVM>? RecruitFooterSettingVMList { get; set; }
    public List<RecruiterSettingVM>? RecruiterSettingVMList { get; set; }
    public RecruitNotificationSettingVM? RecruitNotificationSettingVMList { get; set; }
    public List<RecruitJobApplicationStatusSettingVM>? RecruitJobApplicationStatusSettingVMList { get; set; }
    public List<RecruitCustomQuestionSettingVM>? RecruitCustomQuestionSettingVMList { get; set; }
}