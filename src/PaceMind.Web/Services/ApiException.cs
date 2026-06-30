namespace PaceMind.Web.Services;

/// <summary>Raised when the API responds with a non-success status.</summary>
public sealed class ApiException(string message) : Exception(message);
