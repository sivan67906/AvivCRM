using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class PaymentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PaymentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Payment(string searchQuery = null!)
    {
        ViewData["pTitle"] = "Payments Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Payment";
        ViewData["bChild"] = "Payment View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<PaymentVM>> paymentList = new();

        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all products if no search query is provided
            paymentList =
                await client.GetFromJsonAsync<ApiResultResponse<List<PaymentVM>>>("Payment/all-payment");
        }
        else
        {
            // Fetch products matching the search query
            paymentList =
                await client.GetFromJsonAsync<ApiResultResponse<List<PaymentVM>>>(
                    $"Payment/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query

        //ViewBag.ApiResult = paymentList!.Data;
        //ViewBag.ApiMessage = paymentList!.Message;
        //ViewBag.ApiStatus = paymentList.IsSuccess;
        return View(paymentList!.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        PaymentVM payment = new();
        return PartialView("_Create", payment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PaymentVM payment)
    {
        ApiResultResponse<PaymentVM> payments = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (payment.Name == null)
        //{
        //    payment.Name = "";
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonpayment = JsonConvert.SerializeObject(payment);
        StringContent? paymentcontent = new(jsonpayment, Encoding.UTF8, "application/json");
        HttpResponseMessage? responsePayment =
            await client.PostAsync("Payment/create-payment", paymentcontent);

        if (responsePayment.IsSuccessStatusCode)
        {
            string? jsonResponsePayment = await responsePayment.Content.ReadAsStringAsync();
            payments = JsonConvert.DeserializeObject<ApiResultResponse<PaymentVM>>(jsonResponsePayment);
        }
        else
        {
            string? errorContent = await responsePayment.Content.ReadAsStringAsync();
            payments = new ApiResultResponse<PaymentVM>
            {
                IsSuccess = false,
                Message = responsePayment.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = payment!.Data;
        //ViewBag.ApiMessage = payment!.Message;
        //ViewBag.ApiStatus = payment.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = payment!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!payments!.IsSuccess)
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
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<PaymentVM> payment = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        payment =
            await client.GetFromJsonAsync<ApiResultResponse<PaymentVM>>("Payment/byid-payment/?Id=" + Id);

        //ViewBag.ApiResult = payment!.Data;
        //ViewBag.ApiMessage = payment!.Message;
        //ViewBag.ApiStatus = payment.IsSuccess;

        if (!payment!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", payment.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PaymentVM payment)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (payment.Name == null)
        //{
        //    payment.Name = "";
        //}

        ApiResultResponse<PaymentVM> payments = new();

        if (GuidExtensions.IsNullOrEmpty(payment.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonpayment = JsonConvert.SerializeObject(payment);
        StringContent? paymentcontent = new(jsonpayment, Encoding.UTF8, "application/json");
        HttpResponseMessage? responsePayment =
            await client.PutAsync("Payment/update-payment/", paymentcontent);
        if (responsePayment.IsSuccessStatusCode)
        {
            string? jsonResponsePayment = await responsePayment.Content.ReadAsStringAsync();
            payments = JsonConvert.DeserializeObject<ApiResultResponse<PaymentVM>>(jsonResponsePayment);
        }
        else
        {
            string? errorContent = await responsePayment.Content.ReadAsStringAsync();
            payments = new ApiResultResponse<PaymentVM>
            {
                IsSuccess = false,
                Message = responsePayment.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = payment!.Data;
        //ViewBag.ApiMessage = payment!.Message;
        //ViewBag.ApiStatus = payment.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = payment!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!payments!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid Id)
    {
        //if (GuidExtensions.IsNullOrEmpty(Id)) return View();
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<PaymentVM> payment = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responsePayment = await client.DeleteAsync("Payment/delete-payment?Id=" + Id);
        if (responsePayment.IsSuccessStatusCode)
        {
            string? jsonResponsePayment = await responsePayment.Content.ReadAsStringAsync();
            payment = JsonConvert.DeserializeObject<ApiResultResponse<PaymentVM>>(jsonResponsePayment);
        }
        else
        {
            string? errorContent = await responsePayment.Content.ReadAsStringAsync();
            payment = new ApiResultResponse<PaymentVM>
            {
                IsSuccess = false,
                Message = responsePayment.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = payment!.Data;
        //ViewBag.ApiMessage = payment!.Message;
        //ViewBag.ApiStatus = payment.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = payment!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!payment!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
}