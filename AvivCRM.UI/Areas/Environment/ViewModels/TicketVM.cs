using System.ComponentModel.DataAnnotations;

namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class TicketVM
{
    public List<TicketGroupVM>? TicketGroupsVMList { get; set; }
    public List<TicketTypeVM>? TicketTypesVMList { get; set; }
    public List<TicketAgentVM>? TicketAgentsVMList { get; set; }
    public List<TicketChannelVM>? TicketChannelVMList { get; set; }
    public List<TicketReplyTemplateVM>? TicketReplyTemplatesVMList { get; set; }
}

public class TicketGroupVM
{
    public Guid Id { get; set; }
    public string? Code { get; set; }

    [Required(ErrorMessage = "Ticket Group Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Ticket Group Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Ticket Group Name should not be less than 3 characters")]
    public string? Name { get; set; }
}

public class TicketTypeVM
{
    public Guid Id { get; set; }
    public string? Code { get; set; }

    [Required(ErrorMessage = "Ticket Type Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Ticket Type Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Ticket Type Name should not be less than 3 characters")]
    public string? Name { get; set; }
}

public class TicketAgentVM
{
    public Guid Id { get; set; }
    public string? Code { get; set; }

    [Required(ErrorMessage = "Ticket Agent Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Ticket Agent Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Ticket Agent Name should not be less than 3 characters")]
    public string? Name { get; set; }

    public Guid AgentGroupId { get; set; }
    public string? AgentGroup { get; set; }
    public Guid AgentTypeId { get; set; }
    public string? AgentType { get; set; }
    public Guid AgentStatusId { get; set; }
    public string? AgentStatusName { get; set; }

    public TicketGroupDDSettingVM? TicketGroupDDSetting { get; set; }
    public TicketTypeDDSettingVM? TicketTypeDDSetting { get; set; }
    public TicketToggleStatusVM? TicketToggleStatus { get; set; }
}

public class TicketGroupDDSettingVM
{
    public TicketGroupVM? TicketGroup { get; set; }
    public Guid SelectedId { get; set; }
    public List<TicketGroupVM>? TicketGroupList { get; set; }
}
public class TicketTypeDDSettingVM
{
    public TicketTypeVM? TicketType { get; set; }
    public Guid SelectedId { get; set; }
    public List<TicketTypeVM>? TicketTypeList { get; set; }
}
public class TicketToggleStatusVM
{
    public ToggleValueVM? ToggleValue { get; set; }
    public Guid SelectedId { get; set; }
    public List<ToggleValueVM>? ToggleValueList { get; set; }
}
public class TicketChannelVM
{
    public Guid Id { get; set; }
    public string? Code { get; set; }

    [Required(ErrorMessage = "Ticket Channel Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Ticket Channel Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Ticket Channel Name should not be less than 3 characters")]
    public string? Name { get; set; }
}

public class TicketReplyTemplateVM
{
    public Guid Id { get; set; }
    public string? Code { get; set; }

    [Required(ErrorMessage = "Ticket ReplyTemplate Name should not be empty")]
    [MaxLength(25, ErrorMessage = "Ticket ReplyTemplate Name must not exceed 25 characters")]
    [MinLength(3, ErrorMessage = "Ticket ReplyTemplate Name should not be less than 3 characters")]
    public string? Name { get; set; }
}