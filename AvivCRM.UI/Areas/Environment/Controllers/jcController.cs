using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class jcController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public jcController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> jc()

    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");


        ApiResultResponse<List<JobApplicationCategoryVM>>? financeInvoiceSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationCategoryVM>>>(
                "JobApplicationCategory/all-jobapplicationcategory");
        JobApplicationCategoryVM? financeInvoiceSetting = financeInvoiceSettings!.Data!.FirstOrDefault();
        return View();
    }
}
