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

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");


        ApiResultResponse<List<FinanceInvoiceSettingVM>>? financeInvoiceSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<FinanceInvoiceSettingVM>>>(
                "FinanceInvoiceSetting/all-financeInvoiceSetting");
        FinanceInvoiceSettingVM? financeInvoiceSetting = financeInvoiceSettings!.Data!.FirstOrDefault();

        List<FICBGeneralSettingVM>? cbGeneralSettingItems = financeInvoiceSetting != null
            ? JsonConvert.DeserializeObject<List<FICBGeneralSettingVM>>(financeInvoiceSetting!.FICBGeneralJsonSettings!)
            : new List<FICBGeneralSettingVM>();
        if (cbGeneralSettingItems!.Count > 0)
        {
            financeInvoiceSetting!.FICBGeneralSettings = cbGeneralSettingItems!;
        }

        List<FICBClientInfoSettingVM>? cbClientInfoItems = financeInvoiceSetting != null
            ? JsonConvert.DeserializeObject<List<FICBClientInfoSettingVM>>(financeInvoiceSetting!
                .FICBClientInfoJsonSettings!)
            : new List<FICBClientInfoSettingVM>();
        if (cbClientInfoItems!.Count > 0)
        {
            financeInvoiceSetting!.FICBClientInfoSettings = cbClientInfoItems;
        }

        ApiResultResponse<List<LanguageVM>>? languageList =
            await client.GetFromJsonAsync<ApiResultResponse<List<LanguageVM>>>("Language/all-language");
        ApiResultResponse<LanguageVM>? language =
            await client.GetFromJsonAsync<ApiResultResponse<LanguageVM>>("Language/byid-language/?Id=" +
                                                                         financeInvoiceSetting!.FILanguageId);
        LanguageVM? lang = language!.Data;
        LanguageDDSettingVM? languageDDValue = new()
        {
            language = lang,
            SelectedLanguageId = lang!.Id,
            languageItems = languageList!.Data!.Select(i => new LanguageVM
            {
                Id = i.Id,
                LanguageName = i.LanguageName
            }).ToList()
        };
        financeInvoiceSetting!.LanguageDDSettings = languageDDValue;

        ApiResultResponse<List<FinanceInvoiceTemplateSettingVM>>? financeInvoiceTemplateSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<FinanceInvoiceTemplateSettingVM>>>(
                "FinanceInvoiceTemplateSetting/all-financeinvoicetemplatesetting");
        FinanceInvoiceTemplateSettingVM? financeInvoiceTemplateSetting =
            financeInvoiceTemplateSettings!.Data!.FirstOrDefault();

        List<FIRBTemplateSettingVM>? rbTemplateItems = financeInvoiceTemplateSetting != null
            ? JsonConvert.DeserializeObject<List<FIRBTemplateSettingVM>>(financeInvoiceTemplateSetting!
                .FIRBTemplateJsonSettings!)
            : new List<FIRBTemplateSettingVM>();
        if (rbTemplateItems!.Count > 0)
        {
            financeInvoiceTemplateSetting!.FIRBTemplateSettings = rbTemplateItems;
        }

        ApiResultResponse<List<FinancePrefixSettingVM>>? financePrefixSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<FinancePrefixSettingVM>>>(
                "FinancePrefixSetting/all-financeprefixsetting");
        FinancePrefixSettingVM? financePrefixSetting = financePrefixSettings!.Data!.FirstOrDefault();

        List<FICBPrefixSettingVM>? cbPrefixItems = financePrefixSetting != null
            ? JsonConvert.DeserializeObject<List<FICBPrefixSettingVM>>(financePrefixSetting.FICBPrefixJsonSettings!)
            : new List<FICBPrefixSettingVM>();
        FICBPrefixSettingVM? cbPrefixItem = new();
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

        ApiResultResponse<List<FinanceUnitSettingVM>>? financeUnitSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<FinanceUnitSettingVM>>>(
                "FinanceUnitSetting/all-financeunitsetting");
        List<FinanceUnitSettingVM>? financeUnitSettingList = financeUnitSettings!.Data;

        FinanceVM? viewModel = new()
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
        if (financeInvoiceSetting.FILogoImage != null && financeInvoiceSetting.FILogoImage.Length > 0)
        {
            string? fileName = Path.GetFileName(financeInvoiceSetting.FILogoImage.FileName);
            string? filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (FileStream? stream = new(filePath, FileMode.Create))
            {
                await financeInvoiceSetting.FILogoImage.CopyToAsync(stream);
            }

            financeInvoiceSetting.FILogoPath = "/images/" + fileName;
            financeInvoiceSetting.FILogoImageFileName = fileName;
        }

        if (financeInvoiceSetting.FIAuthorisedImage != null && financeInvoiceSetting.FIAuthorisedImage.Length > 0)
        {
            string? fileName = Path.GetFileName(financeInvoiceSetting.FIAuthorisedImage.FileName);
            string? filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (FileStream? stream = new(filePath, FileMode.Create))
            {
                await financeInvoiceSetting.FIAuthorisedImage.CopyToAsync(stream);
            }

            financeInvoiceSetting.FIAuthorisedImagePath = "/images/" + fileName;
            financeInvoiceSetting.FIAuthorisedImageFileName = fileName;
        }

        ApiResultResponse<FinanceInvoiceSettingVM> fStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? financeInvoiceSettingResponse =
            await client.PutAsJsonAsync("FinanceInvoiceSetting/update-financeInvoiceSetting/", financeInvoiceSetting);
        if (financeInvoiceSettingResponse.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await financeInvoiceSettingResponse.Content.ReadAsStringAsync();
            fStatus = JsonConvert.DeserializeObject<ApiResultResponse<FinanceInvoiceSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await financeInvoiceSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<FinanceInvoiceSettingVM>
            {
                IsSuccess = false,
                Message =
                    financeInvoiceSettingResponse.StatusCode
                        .ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new();
        string serverErrorMessage = fStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> FinanceInvoiceTemplateSettingUpdate(
        FinanceInvoiceTemplateSettingVM financeInvoiceTemplateSetting, string jsonData, Guid Id)
    {
        financeInvoiceTemplateSetting.FIRBTemplateJsonSettings = jsonData;
        financeInvoiceTemplateSetting.Id = Id;

        ApiResultResponse<FinanceInvoiceTemplateSettingVM> fStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? financeInvoiceTemplateSettingResponse = await client.PutAsJsonAsync(
            "FinanceInvoiceTemplateSetting/update-financeinvoicetemplatesetting/", financeInvoiceTemplateSetting);
        if (financeInvoiceTemplateSettingResponse.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await financeInvoiceTemplateSettingResponse.Content.ReadAsStringAsync();
            fStatus =
                JsonConvert.DeserializeObject<ApiResultResponse<FinanceInvoiceTemplateSettingVM>>(
                    jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await financeInvoiceTemplateSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<FinanceInvoiceTemplateSettingVM>
            {
                IsSuccess = false,
                Message =
                    financeInvoiceTemplateSettingResponse.StatusCode
                        .ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new();
        string serverErrorMessage = fStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> FinancePrefixSettingUpdate(FinancePrefixSettingVM financePrefixSetting)
    {
        string? jsonString = "[" + financePrefixSetting.FICBPrefixJsonSettings + "]";
        financePrefixSetting.FICBPrefixJsonSettings = jsonString;

        ApiResultResponse<FinancePrefixSettingVM> fStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? financePrefixSettingResponse =
            await client.PutAsJsonAsync("FinancePrefixSetting/update-financeprefixsetting/", financePrefixSetting);
        if (financePrefixSettingResponse.IsSuccessStatusCode)
        {
            string? jsonfinancePrefixSettingSource = await financePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus =
                JsonConvert.DeserializeObject<ApiResultResponse<FinancePrefixSettingVM>>(
                    jsonfinancePrefixSettingSource);
        }
        else
        {
            string? errorContent = await financePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<FinancePrefixSettingVM>
            {
                IsSuccess = false,
                Message =
                    financePrefixSettingResponse.StatusCode
                        .ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new();
        string serverErrorMessage = fStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> EditFinanceUnitSetting(Guid Id)
    {
        //if (Id == 0) return View();
        ApiResultResponse<FinanceUnitSettingVM> financeUnit = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        financeUnit =
            await client.GetFromJsonAsync<ApiResultResponse<FinanceUnitSettingVM>>(
                "FinanceUnitSetting/byid-financeunitsetting/?Id=" + Id);

        if (!financeUnit!.IsSuccess)
        {
            return View();
        }

        return PartialView("~/Areas/Environment/Views/Finance/FinanceUnitSetting/_Edit.cshtml", financeUnit.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFinanceUnitSetting(FinanceUnitSettingVM financeUnitSetting)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<FinanceUnitSettingVM> finUnitSetting = new();

        if (GuidExtensions.IsNullOrEmpty(financeUnitSetting.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonFinance = JsonConvert.SerializeObject(financeUnitSetting);
        StringContent? jsonFinanceContent = new(jsonFinance, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseFinanceContent =
            await client.PutAsync("FinanceUnitSetting/update-financeunitsetting/", jsonFinanceContent);
        if (responseFinanceContent.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseFinanceContent.Content.ReadAsStringAsync();
            finUnitSetting =
                JsonConvert.DeserializeObject<ApiResultResponse<FinanceUnitSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseFinanceContent.Content.ReadAsStringAsync();
            finUnitSetting = new ApiResultResponse<FinanceUnitSettingVM>
            {
                IsSuccess = false,
                Message =
                    responseFinanceContent.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!finUnitSetting!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> CreateFinanceUnitSetting()
    {
        FinanceUnitSettingVM financeUnitSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Finance/FinanceUnitSetting/_Create.cshtml", financeUnitSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFinanceUnitSetting(FinanceUnitSettingVM financeUnitSetting)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<FinanceUnitSettingVM> pStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseProjectStatus =
            await client.PostAsJsonAsync("FinanceUnitSetting/create-financeunitsetting", financeUnitSetting);
        if (responseProjectStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<FinanceUnitSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<FinanceUnitSettingVM>
            {
                IsSuccess = false,
                Message = responseProjectStatus.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new();
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }
}