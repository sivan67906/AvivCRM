using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectSettingsComponent;
public class AttendanceSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(AttendanceSettingVM attendanceSetting)
    {
        return View(attendanceSetting);
    }
}