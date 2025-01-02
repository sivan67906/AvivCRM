using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Finance.FinanceInvoiceSettingComponent;

public class FinanceInvoiceSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(FinanceInvoiceSettingVM financeInvoiceSetting)
    {
        return View(financeInvoiceSetting);
    }
}



