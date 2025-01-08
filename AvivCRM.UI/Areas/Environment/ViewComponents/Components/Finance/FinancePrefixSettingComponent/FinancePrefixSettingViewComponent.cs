using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Finance.FinancePrefixSettingComponent;
public class FinancePrefixSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(FinancePrefixSettingVM financePrefixSetting)
    {
        return View(financePrefixSetting);
    }
}