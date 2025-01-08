using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;
public class RecruitFooterSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<RecruitFooterSettingVM> recruitFooterSettings)
    {
        return View(recruitFooterSettings);
    }
}