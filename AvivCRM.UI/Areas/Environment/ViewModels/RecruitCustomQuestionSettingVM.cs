namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitCustomQuestionSettingVM
{
    public Guid Id { get; set; }
    public string? CQQuestion { get; set; }
    public Guid CustomQuestionTypeId { get; set; }
    public string? CustomQuestionTypeName { get; set; }
    public Guid CustomQuestionCategoryId { get; set; }
    public string? CustomQuestionCategoryName { get; set; }
    public Guid CQStatusId { get; set; }
    public string? CQStatusName { get; set; }
    public bool CQIsRequiredId { get; set; }
}