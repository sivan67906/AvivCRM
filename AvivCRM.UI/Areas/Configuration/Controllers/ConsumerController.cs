using AvivCRM.UI.Areas.Configuration.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Consumer.Controllers;
[Area("Configuration")]
public class ConsumerController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ConsumerController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Consumer()
    {
        // Page Title
        ViewData["pTitle"] = "Consumers Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Consumer";
        ViewData["bChild"] = "Consumer";
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<ConsumerVM> consumerList = new();



        if (TempData["SearchData"] == null)
        {
            consumerList = await client.GetFromJsonAsync<List<ConsumerVM>>("Consumer/GetAll");
            ViewBag.SearchData = consumerList;
        }
        else
        {
            consumerList = JsonConvert.DeserializeObject<List<ConsumerVM>>(TempData["SearchData"].ToString());
            ViewBag.SearchData = consumerList;
        }

        var planTypes = await client.GetFromJsonAsync<List<PlanTypeVM>>("PlanType/GetAll");
        ViewBag.PlanType = planTypes;
        return View();
    }

    public async Task<IActionResult> Consumer1(List<ConsumerVM> consumer)
    {
        // Page Title
        ViewData["pTitle"] = "Consumers Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Consumer";
        ViewData["bChild"] = "Consumer";
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //var consumerList = await client.GetFromJsonAsync<List<ConsumerVM>>("Consumer/GetAll");
        var planTypes = await client.GetFromJsonAsync<List<PlanTypeVM>>("PlanType/GetAll");
        ViewBag.PlanType = planTypes;
        return View(consumer);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ConsumerVM consumer = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var planTypes = await client.GetFromJsonAsync<List<PlanTypeVM>>("PlanType/GetAll");
        ViewBag.PlanType = planTypes;
        return PartialView("_Create", consumer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ConsumerVM consumer)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var planTypes = await client.GetFromJsonAsync<List<PlanTypeVM>>("PlanType/GetAll");
        ViewBag.PlanType = planTypes;
        await client.PostAsJsonAsync<ConsumerVM>("Consumer/Create", consumer);
        return RedirectToAction("Consumer");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var planTypes = await client.GetFromJsonAsync<List<PlanTypeVM>>("PlanType/GetAll");
        ViewBag.PlanType = planTypes;
        var consumer = await client.GetFromJsonAsync<ConsumerVM>("Consumer/GetById/?Id=" + Id);
        return PartialView("_Edit", consumer);
    }
    [HttpGet]
    public async Task<IActionResult> SearchByName(string searchByName)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var consumers = await client.GetFromJsonAsync<List<ConsumerVM>>("Consumer/search?consumerName=" + searchByName);
        TempData["SearchData"] = JsonConvert.SerializeObject(consumers);
        return RedirectToAction("Consumer");
    }
    [HttpGet]
    public async Task<IActionResult> SearchByPhoneNumber(string searchByPhoneNumber)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var consumers = await client.GetFromJsonAsync<List<ConsumerVM>>("Consumer/GetSearchByPhoneNumber/?consumerPhoneNumber=" + searchByPhoneNumber);
        TempData["SearchData"] = JsonConvert.SerializeObject(consumers);
        return View(consumers);
    }
    [HttpGet]
    public async Task<IActionResult> SearchByDate(string searchDate)
    {
        ConsumerVM consumerVM = new();
        var queryString = $"?searchDate={searchDate}";
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //var consumers = await client.GetFromJsonAsync<List<ConsumerVM>>("Consumer/searchDate1/?searchDate=" + searchDate);
        var consumers = await client.GetFromJsonAsync<List<ConsumerVM>>("Consumer/searchDate1/{queryString}");
        TempData["SearchData"] = JsonConvert.SerializeObject(consumers);
        return View(consumers);
    }
    [HttpGet]
    public async Task<IActionResult> SearchByDateBetween(string startDate, string endDate)
    {
        ConsumerVM consumerVM = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var consumers = await client.GetFromJsonAsync<List<ConsumerVM>>("Consumer/GetSearchByDateBetween/?startDate=" + startDate + "&endDate=" + endDate);
        TempData["SearchData"] = JsonConvert.SerializeObject(consumers);
        return View(consumers);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ConsumerVM consumer)
    {
        if (consumer.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<ConsumerVM>("Consumer/Update/", consumer);
        return RedirectToAction("Consumer");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var consumer = await client.GetFromJsonAsync<ConsumerVM>("Consumer/GetById/?Id=" + Id);
        var planTypes = await client.GetFromJsonAsync<List<PlanTypeVM>>("PlanType/GetAll");
        ViewBag.PlanType = planTypes;
        return PartialView("_Delete", consumer);
    }


    [HttpPost]
    public async Task<IActionResult> Delete(ConsumerVM consumer)
    {
        if (consumer.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Consumer/Delete?Id=" + consumer.Id);
        return RedirectToAction("Consumer");
    }


    //[HttpPost]
    //public async Task<IActionResult> Delete(ConsumerVM consumer)
    //{
    //    JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    //    {
    //        WriteIndented = true
    //    };
    //    string forecastJson = JsonSerializer.Serialize<ConsumerVM>(consumer, options);

    //    if (consumer.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    //client.DeleteAsync("Consumer/Delete" + )
    //    var consumerList = Deletewithresponse(client.BaseAddress.AbsoluteUri + "Consumer/Delete", consumer);
    //    return RedirectToAction("Consumer");
    //}

    //public async Task<HttpResponseMessage> Deletewithresponse(string url, object entity)
    //{
    //    using (var client = new HttpClient())
    //    {
    //        var json = JsonSerializer.Serialize(entity);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");

    //        var request = new HttpRequestMessage
    //        {
    //            Method = HttpMethod.Delete,
    //            RequestUri = new Uri(url),
    //            Content = content
    //        };
    //        return await client.SendAsync(request);
    //    }
    //}



}



