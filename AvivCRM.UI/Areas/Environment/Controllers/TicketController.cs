using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> Ticket()
    {
        // Page Title
        ViewData["pTitle"] = "Tickets Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Ticket";
        ViewData["bChild"] = "Ticket";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        //var ticketAgents = await client.GetFromJsonAsync<List<TicketAgentVM>>("TicketAgent/GetAll");
        //var ticketGroups = await client.GetFromJsonAsync<List<TicketGroupVM>>("TicketGroup/GetAll");
        //var ticketTypes = await client.GetFromJsonAsync<List<TicketTypeVM>>("TicketType/GetAll");
        //var ticketChannels = await client.GetFromJsonAsync<List<TicketChannelVM>>("TicketChannel/GetAll");
        //var ticketReplyTemplates = await client.GetFromJsonAsync<List<TicketReplyTemplateVM>>("TicketReplyTemplate/GetAll");

        var ticketAgents = new List<TicketAgentVM>();
        var ticketGroups = new List<TicketGroupVM>();
        var ticketTypes = new List<TicketTypeVM>();
        var ticketChannels = new List<TicketChannelVM>();
        var ticketReplyTemplates = new List<TicketReplyTemplateVM>();



        var tickets = new TicketVM
        {
            TicketAgentsVMList = ticketAgents,
            TicketGroupsVMList = ticketGroups,
            TicketTypesVMList = ticketTypes,
            TicketChannelVMList = ticketChannels,
            TicketReplyTemplatesVMList = ticketReplyTemplates
        };
        return View(tickets);
    }
}



