using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketAgentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketAgentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TicketAgent()
    {
        // Page Title
        ViewData["pTitle"] = "TicketAgents Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TicketAgent";
        ViewData["bChild"] = "TicketAgent";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TicketAgentVM>>? ticketAgentList =
            await client.GetFromJsonAsync<ApiResultResponse<List<TicketAgentVM>>>("TicketAgent/all-ticketagent");
        List<TicketAgentVM> ticketAgents = ticketAgentList!.Data!;

        ApiResultResponse<List<TicketGroupVM>>? ticketGroupList = await client.GetFromJsonAsync<ApiResultResponse<List<TicketGroupVM>>>("TicketGroup/all-ticketgroup");
        List<TicketGroupVM> ticketGroups = ticketGroupList!.Data!;

        ApiResultResponse<List<TicketTypeVM>>? ticketTypeList = await client.GetFromJsonAsync<ApiResultResponse<List<TicketTypeVM>>>("TicketType/all-tickettype");
        List<TicketTypeVM> ticketTypes = ticketTypeList!.Data!;

        ApiResultResponse<List<ToggleValueVM>>? toggleList = await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");
        List<ToggleValueVM> toggles = toggleList!.Data!;

        foreach (TicketGroupVM parent in ticketGroups)
        {
            foreach (TicketAgentVM? child in ticketAgents.Where(c => c.AgentGroupId == parent.Id))
            {
                child.AgentGroup = parent.Name;
            }
        }
        foreach (TicketTypeVM parent in ticketTypes)
        {
            foreach (TicketAgentVM? child in ticketAgents.Where(c => c.AgentTypeId == parent.Id))
            {
                child.AgentType = parent.Name;
            }
        }
        foreach (ToggleValueVM parent in toggles)
        {
            foreach (TicketAgentVM? child in ticketAgents.Where(c => c.AgentStatusId == parent.Id))
            {
                if (parent.Name == "Yes")
                {
                    child.AgentStatusName = "Enabled";
                }
                else
                {
                    child.AgentStatusName = "Disabled";
                }
            }
        }

        return PartialView("_TicketAgent", ticketAgents);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        TicketAgentVM ticketAgent = new();

        ApiResultResponse<List<TicketGroupVM>>? ticketGroupList = await client.GetFromJsonAsync<ApiResultResponse<List<TicketGroupVM>>>("TicketGroup/all-ticketgroup");
        List<TicketGroupVM> ticketGroups = ticketGroupList!.Data!;

        ApiResultResponse<List<TicketTypeVM>>? ticketTypeList = await client.GetFromJsonAsync<ApiResultResponse<List<TicketTypeVM>>>("TicketType/all-tickettype");
        List<TicketTypeVM> ticketTypes = ticketTypeList!.Data!;

        ApiResultResponse<List<ToggleValueVM>>? toggleList = await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");
        List<ToggleValueVM> toggles = toggleList!.Data!;

        foreach (ToggleValueVM parent in toggles)
        {
            if (parent.Name == "Yes") { parent.Name = "Enable"; }
            else
            {
                parent.Name = "Disable";
            }
        }

        ticketAgent.TicketGroupDDSetting = new();
        ticketAgent.TicketTypeDDSetting = new();
        ticketAgent.TicketToggleStatus = new();

        ticketAgent.TicketGroupDDSetting!.TicketGroupList = new List<TicketGroupVM>(ticketGroups);
        ticketAgent.TicketTypeDDSetting!.TicketTypeList = new List<TicketTypeVM>(ticketTypes);
        ticketAgent.TicketToggleStatus!.ToggleValueList = new List<ToggleValueVM>(toggles);

        return PartialView("_Create", ticketAgent);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketAgentVM ticketAgent)
    {
        if (GuidExtensions.IsNullOrEmpty(ticketAgent.AgentGroupId))
        {
            ModelState.Remove(nameof(ticketAgent.AgentGroupId));
            ModelState.AddModelError("AgentGroupId", "Please select a Group");
        }
        if (GuidExtensions.IsNullOrEmpty(ticketAgent.AgentTypeId))
        {
            ModelState.Remove(nameof(ticketAgent.AgentTypeId));
            ModelState.AddModelError("AgentTypeId", "Please select a Type");
        }
        if (GuidExtensions.IsNullOrEmpty(ticketAgent.AgentStatusId))
        {
            ModelState.Remove(nameof(ticketAgent.AgentStatusId));
            ModelState.AddModelError("AgentStatusId", "Please select a Status");
        }
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<TicketAgentVM> source = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonticketAgent = JsonConvert.SerializeObject(ticketAgent);
        StringContent? ticketAgentcontent = new(jsonticketAgent, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketAgent =
            await client.PostAsync("TicketAgent/create-ticketagent", ticketAgentcontent);

        if (responseTicketAgent.IsSuccessStatusCode)
        {
            string? jsonResponseTicketAgent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketAgentVM>>(jsonResponseTicketAgent);
        }
        else
        {
            string? errorContent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketAgentVM>
            {
                IsSuccess = false,
                Message = responseTicketAgent.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!source!.IsSuccess)
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

        ApiResultResponse<TicketAgentVM>? ticketAgentResponse = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ticketAgentResponse = await client.GetFromJsonAsync<ApiResultResponse<TicketAgentVM>>("TicketAgent/byid-ticketagent/?Id=" + Id);
        TicketAgentVM ticketAgent = ticketAgentResponse!.Data!;

        ApiResultResponse<List<TicketGroupVM>>? ticketGroupList = await client.GetFromJsonAsync<ApiResultResponse<List<TicketGroupVM>>>("TicketGroup/all-ticketgroup");
        List<TicketGroupVM> ticketGroups = ticketGroupList!.Data!;

        ApiResultResponse<List<TicketTypeVM>>? ticketTypeList = await client.GetFromJsonAsync<ApiResultResponse<List<TicketTypeVM>>>("TicketType/all-tickettype");
        List<TicketTypeVM> ticketTypes = ticketTypeList!.Data!;

        ApiResultResponse<List<ToggleValueVM>>? toggleList = await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");
        List<ToggleValueVM> toggles = toggleList!.Data!;
        foreach (ToggleValueVM parent in toggles)
        {
            if (parent.Name == "Yes") { parent.Name = "Enable"; }
            else
            {
                parent.Name = "Disable";
            }
        }

        TicketGroupDDSettingVM ticketGroupDDSetting = new()
        {
            TicketGroup = null,
            SelectedId = ticketAgent.AgentGroupId,
            TicketGroupList = ticketGroups
        };

        TicketTypeDDSettingVM ticketTypeDDSetting = new()
        {
            TicketType = null,
            SelectedId = ticketAgent.AgentTypeId,
            TicketTypeList = ticketTypes
        };
        TicketToggleStatusVM ticketToggleStatus = new()
        {
            ToggleValue = null,
            SelectedId = ticketAgent.Id,
            ToggleValueList = toggles
        };
        ticketAgent.TicketGroupDDSetting = ticketGroupDDSetting;
        ticketAgent.TicketTypeDDSetting = ticketTypeDDSetting;
        ticketAgent.TicketToggleStatus = ticketToggleStatus;

        return PartialView("_Edit", ticketAgent);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TicketAgentVM ticketAgent)
    {
        if (GuidExtensions.IsNullOrEmpty(ticketAgent.AgentGroupId))
        {
            ModelState.Remove(nameof(ticketAgent.AgentGroupId));
            ModelState.AddModelError("AgentGroupId", "Please select a Group");
        }
        if (GuidExtensions.IsNullOrEmpty(ticketAgent.AgentTypeId))
        {
            ModelState.Remove(nameof(ticketAgent.AgentTypeId));
            ModelState.AddModelError("AgentTypeId", "Please select a Type");
        }
        if (GuidExtensions.IsNullOrEmpty(ticketAgent.AgentStatusId))
        {
            ModelState.Remove(nameof(ticketAgent.AgentStatusId));
            ModelState.AddModelError("AgentStatusId", "Please select a Status");
        }
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<TicketAgentVM> source = new();

        if (GuidExtensions.IsNullOrEmpty(ticketAgent.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonticketAgent = JsonConvert.SerializeObject(ticketAgent);
        StringContent? ticketAgentcontent = new(jsonticketAgent, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketAgent =
            await client.PutAsync("TicketAgent/update-ticketagent/", ticketAgentcontent);
        if (responseTicketAgent.IsSuccessStatusCode)
        {
            string? jsonResponseTicketAgent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketAgentVM>>(jsonResponseTicketAgent);
        }
        else
        {
            string? errorContent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketAgentVM>
            {
                IsSuccess = false,
                Message =
                    responseTicketAgent.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!source!.IsSuccess)
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

        ApiResultResponse<TicketAgentVM> source = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTicketAgent = await client.DeleteAsync("TicketAgent/delete-ticketagent?Id=" + Id);
        if (responseTicketAgent.IsSuccessStatusCode)
        {
            string? jsonResponseTicketAgent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketAgentVM>>(jsonResponseTicketAgent);
        }
        else
        {
            string? errorContent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketAgentVM>
            {
                IsSuccess = false,
                Message = responseTicketAgent.StatusCode.ToString()
            };
        }

        if (!source!.IsSuccess)
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