using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectCategoryComponent;

public class ProjectCategoryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<ProjectCategoryVM> projectCategories)
    {
        return View(projectCategories);
    }
}



