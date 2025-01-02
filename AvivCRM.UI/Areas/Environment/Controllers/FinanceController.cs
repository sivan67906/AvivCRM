using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class FinanceController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FinanceController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> Finance()
    {
        // Page Title
        ViewData["pTitle"] = "Finances Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Finance";
        ViewData["bChild"] = "Finance";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        var financeInvoiceSettings = await client.GetFromJsonAsync<List<FinanceInvoiceSettingVM>>("FinanceInvoiceSetting/GetAll");
        var financeInvoiceSetting = financeInvoiceSettings?.FirstOrDefault();
        var cbGeneralSettingItems = financeInvoiceSetting != null ? JsonConvert.DeserializeObject<List<FICBGeneralSettingVM>>(financeInvoiceSetting.FICBGeneralJsonSettings) : new List<FICBGeneralSettingVM>();
        financeInvoiceSetting.FICBGeneralSettings = cbGeneralSettingItems;
        var cbClientInfoItems = financeInvoiceSetting != null ? JsonConvert.DeserializeObject<List<FICBClientInfoSettingVM>>(financeInvoiceSetting.FICBClientInfoJsonSettings) : new List<FICBClientInfoSettingVM>();
        financeInvoiceSetting.FICBClientInfoSettings = cbClientInfoItems;

        var languageList = await client.GetFromJsonAsync<List<LanguageVM>>("Language/GetAll");
        var language = await client.GetFromJsonAsync<LanguageVM>("Language/GetById/?Id=" + financeInvoiceSetting?.FILanguageId);

        var languageDDValue = new LanguageDDSettingVM
        {
            language = language,
            SelectedLanguageId = language!.Id,
            languageItems = languageList?.Select(i => new LanguageVM
            {
                Id = i.Id,
                LanguageName = i.LanguageName
            }).ToList()
        };
        financeInvoiceSetting!.LanguageDDSettings = languageDDValue;


        var financeInvoiceTemplateSettings = await client.GetFromJsonAsync<List<FinanceInvoiceTemplateSettingVM>>("FinanceInvoiceTemplateSetting/GetAll");
        var financeInvoiceTemplateSetting = financeInvoiceTemplateSettings?.FirstOrDefault();
        var rbTemplateItems = financeInvoiceTemplateSetting != null ? JsonConvert.DeserializeObject<List<FIRBTemplateSettingVM>>(financeInvoiceTemplateSetting.FIRBTemplateJsonSettings) : new List<FIRBTemplateSettingVM>();
        financeInvoiceTemplateSetting.FIRBTemplateSettings = rbTemplateItems;

        var financePrefixSettings = await client.GetFromJsonAsync<List<FinancePrefixSettingVM>>("FinancePrefixSetting/GetAll");
        var financePrefixSetting = financePrefixSettings?.FirstOrDefault();
        var cbPrefixItems = financePrefixSetting != null ? JsonConvert.DeserializeObject<List<FICBPrefixSettingVM>>(financePrefixSetting.FICBPrefixJsonSettings) : new List<FICBPrefixSettingVM>();
        var cbPrefixItem = cbPrefixItems?.FirstOrDefault();

        FICBPrefixSettingVM finalPrefixItems = new();
        finalPrefixItems.FPInvoiceVM = cbPrefixItem.FPInvoiceVM;
        finalPrefixItems.FPOrderVM = cbPrefixItem.FPOrderVM;
        finalPrefixItems.FPCreditNoteVM = cbPrefixItem.FPCreditNoteVM;
        finalPrefixItems.FPEstimationVM = cbPrefixItem.FPEstimationVM;

        financePrefixSetting.FICBPrefixSettings = finalPrefixItems;

        var financeUnitSettings = await client.GetFromJsonAsync<List<FinanceUnitSettingVM>>("FinanceUnitSetting/GetAll");

        var viewModel = new FinanceVM
        {
            FinanceInvoiceSettingVMList = financeInvoiceSetting,
            FinanceInvoiceTemplateSettingVMList = financeInvoiceTemplateSetting,
            FinancePrefixSettingVMList = financePrefixSetting,
            FinanceUnitSettingVMList = financeUnitSettings
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> FinanceInvoiceSettingUpdate(
        FinanceInvoiceSettingVM financeInvoiceSetting)
    {
        if (financeInvoiceSetting.Id == 0) return View();
        //if (financeInvoiceSetting.GeneralCBSettings != null && financeInvoiceSetting.GeneralCBSettings.Count > 0)
        //{
        //    string jsonCBString = JsonConvert.SerializeObject(financeInvoiceSetting.GeneralCBSettings, Formatting.Indented);
        //    financeInvoiceSetting.GeneralCBJsonSettings = jsonCBString.Replace("\n", "").Replace("\r", "");
        //}
        //financeInvoiceSetting.GeneralCBJsonSettings = jsonData;

        if (financeInvoiceSetting.FILogoImage != null && financeInvoiceSetting.FILogoImage.Length > 0)
        {
            var fileName = Path.GetFileName(financeInvoiceSetting.FILogoImage.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await financeInvoiceSetting.FILogoImage.CopyToAsync(stream);
            }

            financeInvoiceSetting.FILogoPath = "/images/" + fileName;
            financeInvoiceSetting.FILogoImageFileName = fileName;
        }
        if (financeInvoiceSetting.FIAuthorisedImage != null && financeInvoiceSetting.FIAuthorisedImage.Length > 0)
        {
            var fileName = Path.GetFileName(financeInvoiceSetting.FIAuthorisedImage.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await financeInvoiceSetting.FIAuthorisedImage.CopyToAsync(stream);
            }

            financeInvoiceSetting.FIAuthorisedImagePath = "/images/" + fileName;
            financeInvoiceSetting.FIAuthorisedImageFileName = fileName;
        }

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("FinanceInvoiceSetting/Update/", financeInvoiceSetting);
        return RedirectToAction("Finance");
    }
    [HttpPost]
    public async Task<IActionResult> FinanceInvoiceTemplateSettingUpdate(FinanceInvoiceTemplateSettingVM financeInvoiceTemplateSetting, string jsonData)
    {
        financeInvoiceTemplateSetting.FIRBTemplateJsonSettings = jsonData;
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("FinanceInvoiceTemplateSetting/Update/", financeInvoiceTemplateSetting);
        return RedirectToAction("Finance");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var financeUnit = await client.GetFromJsonAsync<FinanceUnitSettingVM>("FinanceUnitSetting/GetById/?Id=" + Id);
        return PartialView("_Edit", financeUnit);
    }
    [HttpPost]
    public async Task<IActionResult> Update(FinanceUnitSettingVM financeUnitSetting)
    {
        if (financeUnitSetting.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<FinanceUnitSettingVM>("FinanceUnitSetting/Update/", financeUnitSetting);
        return RedirectToAction("Consumer");
    }
}



