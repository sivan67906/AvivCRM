namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitCustomQuestionSettingVM
{
    public int Id { get; set; }
    public string? CQQuestion { get; set; }
    public int CustomQuestionTypeId { get; set; }
    public string? CustomQuestionTypeName { get; set; }
    public int CustomQuestionCategoryId { get; set; }
    public string? CustomQuestionCategoryName { get; set; }
    public int CQStatusId { get; set; }
    public int CQStatusName { get; set; }
    public int CQIsRequiredId { get; set; }
}