using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;
public class RecruiterSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<RecruiterSettingVM> recruiterSettings)
    {
        return View(recruiterSettings);
    }
}