using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;

public class RecruitGeneralSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(RecruitGeneralSettingVM recruitGeneralSetting)
    {
        return View(recruitGeneralSetting);
    }
}





