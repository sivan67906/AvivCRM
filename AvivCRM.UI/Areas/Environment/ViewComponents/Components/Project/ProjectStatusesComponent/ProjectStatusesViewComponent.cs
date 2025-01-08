using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectStatusesComponent;
public class ProjectStatusesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<ProjectStatusVM> projectStatuses)
    {
        return View(projectStatuses);
    }
}