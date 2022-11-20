using GenerationFakeUserData.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace GenerationFakeUserData.Client.Pages
{
    public partial class Index
    {
        [Inject] private HttpClient HttpClient { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        ConfigureGenerationRequest ConfigureGenerationRequest = new();
        private List<UserInfo> Users = default!;

        private async Task<List<UserInfo>> GenerateFakeUserDataAsync()
        {
            var httpResponseMessage = await HttpClient.PostAsJsonAsync($"api/Generation/ReceiveFakeUserData", ConfigureGenerationRequest);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ConfigureGenerationRequest.Page++;
                return (await httpResponseMessage.Content.ReadFromJsonAsync<List<UserInfo>>())!;
            }
            else
            {
                Snackbar.Add(await httpResponseMessage.Content.ReadAsStringAsync(), Severity.Error);
                return new List<UserInfo>();
            }
        }

        private async Task SetFakeUserDataAsync()
        {
            if (ConfigureGenerationRequest.Region != null && ConfigureGenerationRequest.Region != string.Empty)
            {
                ConfigureGenerationRequest.Page = default;
                Users = null!;
                StateHasChanged();
                Users = await GenerateFakeUserDataAsync();
                Users.AddRange(await GenerateFakeUserDataAsync());
            }
        }

        private async Task OnScrollAsync(ScrollEventArgs e)
        {
            if (e.FirstChildBoundingClientRect.Height - e.ScrollTop <= 500)
            {
                Users.AddRange(await GenerateFakeUserDataAsync());
            }
        }

        private async Task SaveGenerationUserData()
        {
            var httpResponseMessage = await HttpClient.PostAsJsonAsync("api/Generation/SaveGenerationUserData", Users);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var bytesFile = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                using var streamRef = new DotNetStreamReference(stream: new MemoryStream(bytesFile));
                await JSRuntime.InvokeVoidAsync("downloadFileFromStream", "filePersons.csv", streamRef);
            }
            else
            {
                Snackbar.Add(await httpResponseMessage.Content.ReadAsStringAsync(), Severity.Error);
            }
        }
    }
}
