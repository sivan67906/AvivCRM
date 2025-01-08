using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Recruit.RecruitCustomQuestionSettingComponent;
public class RecruitNotificationSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(RecruitNotificationSettingVM recruitNotificationSetting)
    {
        return View(recruitNotificationSetting);
    }
}