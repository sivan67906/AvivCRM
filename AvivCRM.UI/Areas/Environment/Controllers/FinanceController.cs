using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
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


        var financeInvoiceSettings = await client.GetFromJsonAsync<ApiResultResponse<List<FinanceInvoiceSettingVM>>>("FinanceInvoiceSetting/all-financeInvoiceSetting");
        var financeInvoiceSetting = financeInvoiceSettings!.Data!.FirstOrDefault();

        var cbGeneralSettingItems = financeInvoiceSetting != null ? JsonConvert.DeserializeObject<List<FICBGeneralSettingVM>>(financeInvoiceSetting!.FICBGeneralJsonSettings!) : new List<FICBGeneralSettingVM>();
        if (cbGeneralSettingItems!.Count > 0)
        {
            financeInvoiceSetting!.FICBGeneralSettings = cbGeneralSettingItems!;
        }

        var cbClientInfoItems = financeInvoiceSetting != null ? JsonConvert.DeserializeObject<List<FICBClientInfoSettingVM>>(financeInvoiceSetting!.FICBClientInfoJsonSettings!) : new List<FICBClientInfoSettingVM>();
        if (cbClientInfoItems!.Count > 0)
        {
            financeInvoiceSetting!.FICBClientInfoSettings = cbClientInfoItems;
        }

        var languageList = await client.GetFromJsonAsync<ApiResultResponse<List<LanguageVM>>>("Language/all-language");
        var language = await client.GetFromJsonAsync<ApiResultResponse<LanguageVM>>("Language/byid-language/?Id=" + financeInvoiceSetting!.FILanguageId);
        var lang = language!.Data;
        var languageDDValue = new LanguageDDSettingVM
        {
            language = lang,
            SelectedLanguageId = lang!.Id,
            languageItems = languageList!.Data!.Select(i => new LanguageVM
            {
                Id = i.Id,
                LanguageName = i.LanguageName
            }).ToList()
        };
        if (financeInvoiceSetting != null)
        {
            financeInvoiceSetting!.LanguageDDSettings = languageDDValue;
        }

        var financeInvoiceTemplateSettings = await client.GetFromJsonAsync<ApiResultResponse<List<FinanceInvoiceTemplateSettingVM>>>("FinanceInvoiceTemplateSetting/all-financeinvoicetemplatesetting");
        var financeInvoiceTemplateSetting = financeInvoiceTemplateSettings!.Data!.FirstOrDefault();

        var rbTemplateItems = financeInvoiceTemplateSetting != null ? JsonConvert.DeserializeObject<List<FIRBTemplateSettingVM>>(financeInvoiceTemplateSetting!.FIRBTemplateJsonSettings!) : new List<FIRBTemplateSettingVM>();
        if (rbTemplateItems!.Count > 0)
        {
            financeInvoiceTemplateSetting!.FIRBTemplateSettings = rbTemplateItems;
        }

        var financePrefixSettings = await client.GetFromJsonAsync<ApiResultResponse<List<FinancePrefixSettingVM>>>("FinancePrefixSetting/all-financeprefixsetting");
        var financePrefixSetting = financePrefixSettings!.Data!.FirstOrDefault();

        var cbPrefixItems = financePrefixSetting != null ? JsonConvert.DeserializeObject<List<FICBPrefixSettingVM>>(financePrefixSetting.FICBPrefixJsonSettings!) : new List<FICBPrefixSettingVM>();
        var cbPrefixItem = new FICBPrefixSettingVM();
        if (cbPrefixItems!.Count > 0)
        {
            cbPrefixItem = cbPrefixItems?.FirstOrDefault();
        }

        FICBPrefixSettingVM finalPrefixItems = new();
        finalPrefixItems.FPInvoiceVM = cbPrefixItem!.FPInvoiceVM;
        finalPrefixItems.FPOrderVM = cbPrefixItem.FPOrderVM;
        finalPrefixItems.FPCreditNoteVM = cbPrefixItem.FPCreditNoteVM;
        finalPrefixItems.FPEstimationVM = cbPrefixItem.FPEstimationVM;

        if (financePrefixSetting != null)
        {
            financePrefixSetting!.FICBPrefixSettingVM = finalPrefixItems;
        }
        var financeUnitSettings = await client.GetFromJsonAsync<ApiResultResponse<List<FinanceUnitSettingVM>>>("FinanceUnitSetting/all-financeunitsetting");
        var financeUnitSettingList = financeUnitSettings!.Data;

        var viewModel = new FinanceVM
        {
            FinanceInvoiceSettingVMList = financeInvoiceSetting,
            FinanceInvoiceTemplateSettingVMList = financeInvoiceTemplateSetting,
            FinancePrefixSettingVMList = financePrefixSetting,
            FinanceUnitSettingVMList = financeUnitSettingList
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> FinanceInvoiceSettingUpdate(FinanceInvoiceSettingVM financeInvoiceSetting)
    {
        //if (financeInvoiceSetting.Id == 0) return View();


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
        ApiResultResponse<FinanceInvoiceSettingVM> fStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var financeInvoiceSettingResponse = await client.PutAsJsonAsync("FinanceInvoiceSetting/update-financeInvoiceSetting/", financeInvoiceSetting);
        if (financeInvoiceSettingResponse.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await financeInvoiceSettingResponse.Content.ReadAsStringAsync();
            fStatus = JsonConvert.DeserializeObject<ApiResultResponse<FinanceInvoiceSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await financeInvoiceSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<FinanceInvoiceSettingVM>
            {
                IsSuccess = false,
                Message = financeInvoiceSettingResponse.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = fStatus!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> FinanceInvoiceTemplateSettingUpdate(FinanceInvoiceTemplateSettingVM financeInvoiceTemplateSetting, string jsonData, Guid Id)
    {
        financeInvoiceTemplateSetting.FIRBTemplateJsonSettings = jsonData;
        financeInvoiceTemplateSetting.Id = Id;

        ApiResultResponse<FinanceInvoiceTemplateSettingVM> fStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var financeInvoiceTemplateSettingResponse = await client.PutAsJsonAsync("FinanceInvoiceTemplateSetting/update-financeinvoicetemplatesetting/", financeInvoiceTemplateSetting);
        if (financeInvoiceTemplateSettingResponse.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await financeInvoiceTemplateSettingResponse.Content.ReadAsStringAsync();
            fStatus = JsonConvert.DeserializeObject<ApiResultResponse<FinanceInvoiceTemplateSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await financeInvoiceTemplateSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<FinanceInvoiceTemplateSettingVM>
            {
                IsSuccess = false,
                Message = financeInvoiceTemplateSettingResponse.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = fStatus!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> FinancePrefixSettingUpdate(FinancePrefixSettingVM financePrefixSetting)
    {
        var jsonString = "[" + financePrefixSetting.FICBPrefixJsonSettings + "]";
        financePrefixSetting.FICBPrefixJsonSettings = jsonString;

        ApiResultResponse<FinancePrefixSettingVM> fStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var financePrefixSettingResponse = await client.PutAsJsonAsync("FinancePrefixSetting/update-financeprefixsetting/", financePrefixSetting);
        if (financePrefixSettingResponse.IsSuccessStatusCode)
        {
            var jsonfinancePrefixSettingSource = await financePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus = JsonConvert.DeserializeObject<ApiResultResponse<FinancePrefixSettingVM>>(jsonfinancePrefixSettingSource);
        }
        else
        {
            var errorContent = await financePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<FinancePrefixSettingVM>
            {
                IsSuccess = false,
                Message = financePrefixSettingResponse.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = fStatus!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> EditFinanceUnitSetting(Guid Id)
    {
        //if (Id == 0) return View();
        ApiResultResponse<FinanceUnitSettingVM> financeUnit = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        financeUnit = await client.GetFromJsonAsync<ApiResultResponse<FinanceUnitSettingVM>>("FinanceUnitSetting/byid-financeunitsetting/?Id=" + Id);

        if (!financeUnit!.IsSuccess) return View();
        else return PartialView("~/Areas/Environment/Views/Finance/FinanceUnitSetting/_Edit.cshtml", financeUnit.Data);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateFinanceUnitSetting(FinanceUnitSettingVM financeUnitSetting)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        ApiResultResponse<FinanceUnitSettingVM> finUnitSetting = new();

        if (GuidExtensions.IsNullOrEmpty(financeUnitSetting.Id)) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var jsonFinance = JsonConvert.SerializeObject(financeUnitSetting);
        var jsonFinanceContent = new StringContent(jsonFinance, Encoding.UTF8, "application/json");
        var responseFinanceContent = await client.PutAsync("FinanceUnitSetting/update-financeunitsetting/", jsonFinanceContent);
        if (responseFinanceContent.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseFinanceContent.Content.ReadAsStringAsync();
            finUnitSetting = JsonConvert.DeserializeObject<ApiResultResponse<FinanceUnitSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseFinanceContent.Content.ReadAsStringAsync();
            finUnitSetting = new ApiResultResponse<FinanceUnitSettingVM>
            {
                IsSuccess = false,
                Message = responseFinanceContent.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!finUnitSetting!.IsSuccess)
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        else
            return Json(new { success = true });
    }

    //[HttpGet]
    //public async Task<IActionResult> Create()
    //{
    //    FinanceUnitSettingVM financeUnitSetting = new();
    //    return PartialView("~/Areas/Environment/Views/Finance/FinanceUnitSetting/_Create.cshtml", financeUnitSetting);
    //}

    [HttpGet]
    public async Task<IActionResult> CreateFinanceUnitSetting()
    {
        FinanceUnitSettingVM financeUnitSetting = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Finance/FinanceUnitSetting/_Create.cshtml", financeUnitSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFinanceUnitSetting(FinanceUnitSettingVM financeUnitSetting)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        ApiResultResponse<FinanceUnitSettingVM> pStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var responseProjectStatus = await client.PostAsJsonAsync("FinanceUnitSetting/create-financeunitsetting", financeUnitSetting);
        if (responseProjectStatus.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<FinanceUnitSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<FinanceUnitSettingVM>
            {
                IsSuccess = false,
                Message = responseProjectStatus.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }
}



