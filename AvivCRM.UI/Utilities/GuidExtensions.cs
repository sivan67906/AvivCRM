namespace AvivCRM.UI.Utilities;

public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? guid)
    {
        return !guid.HasValue || guid.Value == Guid.Empty;
    }
}

