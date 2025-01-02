using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectSettingsComponent;

public class ProjectSettingsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<ProjectSettingVM> projectSettings)
    {
        return View(projectSettings);
    }
}



