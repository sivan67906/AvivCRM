using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class MessageController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MessageController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Message()
    {
        // Page Title
        ViewData["pTitle"] = "Messages Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Message";
        ViewData["bChild"] = "Message View";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<MessageVM>? MessageList = await client.GetFromJsonAsync<List<MessageVM>>("Message/GetAll");

        return View(MessageList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        MessageVM company = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", company);
    }

    [HttpPost]
    public async Task<IActionResult> Create(MessageVM Message)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<MessageVM>("Message/Create", Message);
        return RedirectToAction("Message");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        MessageVM? Message = await client.GetFromJsonAsync<MessageVM>("Message/GetById/?Id=" + Id);
        return PartialView("_Edit", Message);
    }

    [HttpPost]
    public async Task<IActionResult> Update(MessageVM Message)
    {
        if (Message.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<MessageVM>("Message/Update/", Message);
        return RedirectToAction("Message");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        MessageVM? Message = await client.GetFromJsonAsync<MessageVM>("Message/GetById/?Id=" + Id);
        return PartialView("_Delete", Message);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(MessageVM Message)
    {
        if (Message.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Message/Delete?Id=" + Message.Id);
        return RedirectToAction("Message");
    }
}