using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;
public class RecruitCustomQuestionSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<RecruitCustomQuestionSettingVM> recruitCustomQuestionSettings)
    {
        return View(recruitCustomQuestionSettings);
    }
}