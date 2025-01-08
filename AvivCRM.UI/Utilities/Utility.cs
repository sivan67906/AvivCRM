using System.Text.Json;

namespace AvivCRM.UI.Utilities;
public class Utility
{
    public static Dictionary<string, List<string>> ExtractErrorsFromWebAPIResponse(string body)
    {
        Dictionary<string, List<string>>? response = new();

        JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(body);
        JsonElement errorsJsonElement = jsonElement.GetProperty("errors");
        foreach (JsonProperty fieldWithErrors in errorsJsonElement.EnumerateObject())
        {
            string? field = fieldWithErrors.Name;
            List<string>? errors = new();
            foreach (JsonElement errorKind in fieldWithErrors.Value.EnumerateArray())
            {
                string? error = errorKind.GetString();
                errors.Add(error);
            }

            response.Add(field, errors);
        }

        return response;
    }
}