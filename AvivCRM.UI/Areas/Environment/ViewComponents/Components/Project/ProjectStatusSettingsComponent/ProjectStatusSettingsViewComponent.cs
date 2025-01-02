using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectStatusSettingsComponent;
public class ProjectStatusSettingsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<ProjectStatusVM> projectStatusSettings)
    {
        return View(projectStatusSettings);
    }
}



