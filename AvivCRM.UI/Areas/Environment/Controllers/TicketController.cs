using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketController : Controller
{
    public async Task<IActionResult> Ticket()
    {
        // Page Title
        ViewData["pTitle"] = "Tickets Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Ticket";
        ViewData["bChild"] = "Ticket";

        //var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        // all-ticketagent, byid-ticketagent, create-ticketagent, update-ticketagent, delete-ticketagent
        //var ticketAgents = await client.GetFromJsonAsync<List<TicketAgentVM>>("TicketAgent/all-ticketagent");
        //var ticketGroups = await client.GetFromJsonAsync<List<TicketGroupVM>>("TicketGroup/all-ticketgroup");
        //var ticketTypes = await client.GetFromJsonAsync<List<TicketTypeVM>>("TicketType/all-tickettype");
        //var ticketChannels = await client.GetFromJsonAsync<List<TicketChannelVM>>("TicketChannel/all-ticketchannel");
        //var ticketReplyTemplates = await client.GetFromJsonAsync<List<TicketReplyTemplateVM>>("TicketReplyTemplate/all-ticketreplytemplate");

        //var ticketGroups = new List<TicketGroupVM>();
        //var ticketTypes = new List<TicketTypeVM>();
        //var ticketAgents = new List<TicketAgentVM>();
        //var ticketChannels = new List<TicketChannelVM>();
        //var ticketReplyTemplates = new List<TicketReplyTemplateVM>();

        //var tickets = new TicketVM
        //{
        //    TicketGroupsVMList = ticketGroups,
        //    TicketTypesVMList = ticketTypes,
        //    TicketAgentsVMList = ticketAgents,
        //    TicketChannelVMList = ticketChannels,
        //    TicketReplyTemplatesVMList = ticketReplyTemplates
        //};
        //var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        //var ticketGroups = await client.GetFromJsonAsync<ApiResultResponse<List<TicketGroupVM>>>("TicketGroup/all-ticketgroup");

        //return PartialView("~/Areas/Environment/Views/TicketGroup/_TicketGroup.cshtml", ticketGroups!.Data!.SingleOrDefault());
        return View();
    }
}