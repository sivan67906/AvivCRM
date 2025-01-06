using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectCategoriesComponent;

public class ProjectCategoriesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<ProjectCategoryVM> projectCategories)
    {
        return View(projectCategories);
    }
}



