using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;

public class RecruitFooterSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<RecruitFooterSettingVM> recruitFooterSettings)
    {
        return View(recruitFooterSettings);
    }
}







