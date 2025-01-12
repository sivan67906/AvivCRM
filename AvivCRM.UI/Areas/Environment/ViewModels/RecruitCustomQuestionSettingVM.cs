using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class RecruitCustomQuestionSettingVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Question should not be empty")]
    [MaxLength(250, ErrorMessage = "Question must not exceed 250 characters")]
    [MinLength(3, ErrorMessage = "Question should not be less than 3 characters")]
    public string? CQQuestion { get; set; }
    public Guid CustomQuestionTypeId { get; set; }
    public string? CustomQuestionTypeName { get; set; }
    public Guid CustomQuestionCategoryId { get; set; }
    public string? CustomQuestionCategoryName { get; set; }
    public Guid CQStatusId { get; set; }
    public string? CQStatusName { get; set; }
    public bool CQIsRequiredId { get; set; }

    public CustomQuestionCategoryDDSettingVM? CustomQuestionCategoryDDSetting { get; set; }
    public CustomQuestionTypeDDSettingVM? CustomQuestionTypeDDSetting { get; set; }
    public ToggleDDSettingVM? ToggleDDSetting { get; set; }
}

public class CustomQuestionTypeDDSettingVM
{
    public CustomQuestionTypeVM? CustomQuestionType { get; set; }
    public Guid SelectedId { get; set; }
    public List<CustomQuestionTypeVM>? CustomQuestionTypeList { get; set; }
}

public class CustomQuestionCategoryDDSettingVM
{
    public CustomQuestionCategoryVM? CustomQuestionCategory { get; set; }
    public Guid SelectedId { get; set; }
    public List<CustomQuestionCategoryVM>? CustomQuestionCategoryList { get; set; }
}