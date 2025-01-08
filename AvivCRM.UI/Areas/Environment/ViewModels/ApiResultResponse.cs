namespace AvivCRM.UI.Areas.Environment.ViewModels;
public class ApiResultResponse<TEntity>
{
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }
    public TEntity? Data { get; set; }
}