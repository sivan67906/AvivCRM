using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;
public class RecruitGeneralSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(RecruitGeneralSettingVM recruitGeneralSetting)
    {
        return View(recruitGeneralSetting);
    }
}