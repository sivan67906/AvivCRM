using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.ViewComponents.Components.Finance.FinanceInvoiceTemplateSettingComponent;
public class FinanceInvoiceTemplateSettingViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(FinanceInvoiceTemplateSettingVM financeInvoiceTemplateSetting)
    {
        return View(financeInvoiceTemplateSetting);
    }
}