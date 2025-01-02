using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectSettingsComponent;

public class AttendanceSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(AttendanceSettingVM attendanceSetting)
    {
        return View(attendanceSetting);
    }
}



