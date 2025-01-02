using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;

public class RecruitJobApplicationStatusSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<RecruitJobApplicationStatusSettingVM> recruitJobApplicationStatusSettings)
    {
        return View(recruitJobApplicationStatusSettings);
    }
}













