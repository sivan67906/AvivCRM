using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Finance.FinanceUnitSettingComponent;

public class FinanceUnitSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<FinanceUnitSettingVM> financeUnitSettings)
    {
        return View(financeUnitSettings);
    }
}



