using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;

public class RecruitNotificationSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(RecruitNotificationSettingVM recruitNotificationSetting)
    {
        return View(recruitNotificationSetting);
    }
}











