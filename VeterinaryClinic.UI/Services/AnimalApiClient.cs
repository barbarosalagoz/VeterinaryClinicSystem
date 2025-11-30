using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using VeterinaryClinic.UI.Models;

namespace VeterinaryClinic.UI.Services;

public class AnimalApiClient : IAnimalApiClient
{
    private readonly HttpClient _httpClient;

    public AnimalApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<AnimalListItemViewModel>> GetAnimalsAsync()
    {
        var result =
            await _httpClient.GetFromJsonAsync<List<AnimalListItemViewModel>>("api/Animals");

        return result ?? new List<AnimalListItemViewModel>();
    }
}
