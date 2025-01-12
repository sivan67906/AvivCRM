namespace AvivCRM.UI.Areas.Configuration.ViewModels;

public class ApiResultResponseConfigVM<TEntity>
{
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public TEntity? Data { get; set; }
}