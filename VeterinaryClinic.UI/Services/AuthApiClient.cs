using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VeterinaryClinic.UI.Models.Auth;

namespace VeterinaryClinic.UI.Services;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResponseDto?> LoginAsync(string email, string password)
    {
        var payload = new { Email = email, Password = password };

        var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", payload);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
    }
}
