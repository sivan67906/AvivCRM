using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Finance.FinancePrefixSettingComponent;

public class FinancePrefixSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(FinancePrefixSettingVM financePrefixSetting)
    {
        return View(financePrefixSetting);
    }
}



