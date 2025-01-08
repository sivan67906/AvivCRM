using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Project.ProjectCategoryComponent;
public class EmployeeShiftSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<EmployeeShiftSettingVM> employeeShiftSettings)
    {
        return View(employeeShiftSettings);
    }
}