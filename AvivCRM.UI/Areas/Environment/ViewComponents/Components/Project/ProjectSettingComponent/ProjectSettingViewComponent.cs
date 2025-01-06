using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectSettingComponent;

public class ProjectSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ProjectSettingVM projectSetting)
    {
        return View(projectSetting);
    }
}



