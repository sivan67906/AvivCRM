namespace AvivCRM.UI.Areas.Environment.ViewModels;

public class TicketVM
{
    public List<TicketAgentVM>? TicketAgentsVMList { get; set; }
    public List<TicketGroupVM>? TicketGroupsVMList { get; set; }
    public List<TicketTypeVM>? TicketTypesVMList { get; set; }
    public List<TicketChannelVM>? TicketChannelVMList { get; set; }
    public List<TicketReplyTemplateVM>? TicketReplyTemplatesVMList { get; set; }
}
public class TicketAgentVM
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int AgentGroupId { get; set; }
    public string? AgentGroup { get; set; }
    public int AgentTypeId { get; set; }
    public string? AgentType { get; set; }
    public bool Status { get; set; }
}
public class TicketGroupVM
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
}
public class TicketTypeVM
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
}
public class TicketChannelVM
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
}
public class TicketReplyTemplateVM
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
}



