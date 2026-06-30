using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using PaceMind.Contracts;

namespace PaceMind.Web.Services;

/// <summary>Typed gateway to the PaceMind API. Keeps HTTP and serialization concerns
/// out of the components.</summary>
public sealed class PaceMindApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public async Task<PlanPreviewResponse> PreviewPlanAsync(PlanPreviewRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/plan/preview", request, JsonOptions, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var detail = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new ApiException(string.IsNullOrWhiteSpace(detail) ? "The plan could not be generated." : detail);
        }

        return await response.Content.ReadFromJsonAsync<PlanPreviewResponse>(JsonOptions, cancellationToken)
               ?? throw new ApiException("The API returned an empty plan.");
    }
}
