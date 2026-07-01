using Microsoft.JSInterop;

namespace PaceMind.Web.Services;

public sealed class ThemeService(IJSRuntime js)
{
    public string Current { get; private set; } = "dark";

    public event Action? Changed;

    public async Task InitializeAsync()
    {
        Current = await js.InvokeAsync<string>("pacemindTheme.get");
        Changed?.Invoke();
    }

    public async Task ToggleAsync()
    {
        Current = Current == "dark" ? "light" : "dark";
        await js.InvokeVoidAsync("pacemindTheme.set", Current);
        Changed?.Invoke();
    }
}
