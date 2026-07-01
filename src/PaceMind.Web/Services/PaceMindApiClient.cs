using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using PaceMind.Contracts;

namespace PaceMind.Web.Services;

public sealed class PaceMindApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public async Task<PlanPreviewResponse> PreviewPlanAsync(PlanPreviewRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/plan/preview", request, JsonOptions, cancellationToken);
        await EnsureSuccess(response, "The plan could not be generated.", cancellationToken);

        return await response.Content.ReadFromJsonAsync<PlanPreviewResponse>(JsonOptions, cancellationToken)
               ?? throw new ApiException("The API returned an empty plan.");
    }

    public async Task<PlanAdaptResponse> AdaptPlanAsync(PlanAdaptRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/plan/adapt", request, JsonOptions, cancellationToken);
        await EnsureSuccess(response, "The plan could not be adapted.", cancellationToken);

        return await response.Content.ReadFromJsonAsync<PlanAdaptResponse>(JsonOptions, cancellationToken)
               ?? throw new ApiException("The API returned an empty response.");
    }

    public async Task<PlanWeekResponse> GetWeekAsync(PlanWeekRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/plan/week", request, JsonOptions, cancellationToken);
        await EnsureSuccess(response, "The week could not be generated.", cancellationToken);

        return await response.Content.ReadFromJsonAsync<PlanWeekResponse>(JsonOptions, cancellationToken)
               ?? throw new ApiException("The API returned an empty week.");
    }

    public async Task<CoachReplyResponse> CommentOnWeekAsync(CoachCommentRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/coach/comment", request, JsonOptions, cancellationToken);
        await EnsureSuccess(response, "The coach could not respond.", cancellationToken);

        return await response.Content.ReadFromJsonAsync<CoachReplyResponse>(JsonOptions, cancellationToken)
               ?? throw new ApiException("The API returned an empty response.");
    }

    public async Task<CoachReplyResponse> ChatAsync(CoachChatRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("api/coach/chat", request, JsonOptions, cancellationToken);
        await EnsureSuccess(response, "The coach could not respond.", cancellationToken);

        return await response.Content.ReadFromJsonAsync<CoachReplyResponse>(JsonOptions, cancellationToken)
               ?? throw new ApiException("The API returned an empty response.");
    }

    private static async Task EnsureSuccess(HttpResponseMessage response, string fallback, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var detail = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new ApiException(string.IsNullOrWhiteSpace(detail) ? fallback : detail);
    }
}
