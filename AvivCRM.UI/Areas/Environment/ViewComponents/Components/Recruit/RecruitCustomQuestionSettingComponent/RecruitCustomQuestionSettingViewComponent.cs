using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;

public class RecruitCustomQuestionSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<RecruitCustomQuestionSettingVM> recruitCustomQuestionSettings)
    {
        return View(recruitCustomQuestionSettings);
    }
}















