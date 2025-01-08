using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;
public class RecruitJobApplicationStatusSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<RecruitJobApplicationStatusSettingVM> recruitJobApplicationStatusSettings)
    {
        return View(recruitJobApplicationStatusSettings);
    }
}