using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Area("Environment")]
public class NotificationMainController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationMainController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> NotificationMain()
    {
        // Page Title
        ViewData["pTitle"] = "NotificationMains Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "NotificationMain";
        ViewData["bChild"] = "NotificationMain";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<NotificationMainVM>>? NotificationMains =
            await client.GetFromJsonAsync<ApiResultResponse<List<NotificationMainVM>>>(
                "NotificationMain/all-notificationmain");
        //var NotificationMains = await client.GetFromJsonAsync<List<NotificationMainVM>>("NotificationMain/GetAll");
        NotificationMainVM? NotificationMain = NotificationMains!.Data!.FirstOrDefault();

        List<CommonNotificationMainVM>? CommonNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<CommonNotificationMainVM>>(
                NotificationMain.CommonNotificationMainJson!)
            : [];
        List<LeaveNotificationMainVM>? LeaveNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<LeaveNotificationMainVM>>(NotificationMain.LeaveNotificationMainJson!)
            : [];
        List<ProposalNotificationMainVM>? ProposalNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<ProposalNotificationMainVM>>(NotificationMain
                .ProposalNotificationMainJson!)
            : [];
        List<InvoiceNotificationMainVM>? InvoiceNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<InvoiceNotificationMainVM>>(NotificationMain
                .InvoiceNotificationMainJson!)
            : [];
        List<PaymentNotificationMainVM>? PaymentNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<PaymentNotificationMainVM>>(NotificationMain
                .PaymentNotificationMainJson!)
            : [];
        List<TaskNotificationMainVM>? TaskNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<TaskNotificationMainVM>>(NotificationMain.TaskNotificationMainJson!)
            : [];
        List<TicketNotificationMainVM>? TicketNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<TicketNotificationMainVM>>(
                NotificationMain.TicketNotificationMainJson!)
            : [];
        List<ProjectNotificationMainVM>? ProjectNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<ProjectNotificationMainVM>>(NotificationMain
                .ProjectNotificationMainJson!)
            : [];
        List<ReminderNotificationMainVM>? ReminderNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<ReminderNotificationMainVM>>(NotificationMain
                .ReminderNotificationMainJson!)
            : [];
        List<RequestNotificationMainVM>? RequestNotificationMainItems = NotificationMain != null
            ? JsonConvert.DeserializeObject<List<RequestNotificationMainVM>>(NotificationMain
                .RequestNotificationMainJson!)
            : [];

        NotificationMain!.CommonNotificationMains = CommonNotificationMainItems;
        NotificationMain!.LeaveNotificationMains = LeaveNotificationMainItems;
        NotificationMain!.ProposalNotificationMains = ProposalNotificationMainItems;
        NotificationMain!.InvoiceNotificationMains = InvoiceNotificationMainItems;
        NotificationMain!.PaymentNotificationMains = PaymentNotificationMainItems;
        NotificationMain!.TaskNotificationMains = TaskNotificationMainItems;
        NotificationMain!.TicketNotificationMains = TicketNotificationMainItems;
        NotificationMain!.ProjectNotificationMains = ProjectNotificationMainItems;
        NotificationMain!.ReminderNotificationMains = ReminderNotificationMainItems;
        NotificationMain!.RequestNotificationMains = RequestNotificationMainItems;

        return View(NotificationMain);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateNotificationMain(
        Guid cbValueId, string cbValue1, string cbValue2, string cbValue3, string cbValue4, string cbValue5,
        string cbValue6, string cbValue7, string cbValue8, string cbValue9, string cbValue10)
    {
        //if (cbValueId == "0" || cbValueId == "") return View();
        ApiResultResponse<ProjectSettingVM> result = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        NotificationMainVM notificationMain = new()
        {
            Id = cbValueId,
            CommonNotificationMainJson = cbValue1,
            LeaveNotificationMainJson = cbValue2,
            ProposalNotificationMainJson = cbValue3,
            InvoiceNotificationMainJson = cbValue4,
            PaymentNotificationMainJson = cbValue5,
            TaskNotificationMainJson = cbValue6,
            TicketNotificationMainJson = cbValue7,
            ProjectNotificationMainJson = cbValue8,
            ReminderNotificationMainJson = cbValue9,
            RequestNotificationMainJson = cbValue10
        };

        HttpResponseMessage? responseNotificationMain =
            await client.PutAsJsonAsync("NotificationMain/update-notificationmain/", notificationMain);
        if (responseNotificationMain.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseNotificationMain.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<ApiResultResponse<ProjectSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseNotificationMain.Content.ReadAsStringAsync();
            result = new ApiResultResponse<ProjectSettingVM>
            {
                IsSuccess = false,
                Message =
                    responseNotificationMain.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = result!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!result!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }
}