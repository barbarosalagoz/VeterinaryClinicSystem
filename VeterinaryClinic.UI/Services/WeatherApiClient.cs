using System.Text.Json;
using System.Text.Json.Serialization;
using VeterinaryClinic.UI.Models;

namespace VeterinaryClinic.UI.Services;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    // NOT: Burası normalde appsettings.json'dan okunmalı ama kolaylık olsun diye buraya yazıyoruz.
    // Kendi API Key'ini buraya yapıştırabilirsin.
    private const string ApiKey = "afd64b2e3490cb2f1616ab7627221ef4"; // Test Key (Çalışmazsa yenisini almalısın)
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherViewModel> GetWeatherAsync(string city)
    {
        try
        {
            // URL oluştur: Metric = Santigrat, Lang = Türkçe
            var url = $"{BaseUrl}?q={city}&appid={ApiKey}&units=metric&lang=tr";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // JSON'ı C# nesnesine çevir
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(json);

                if (weatherData != null)
                {
                    return new WeatherViewModel
                    {
                        City = weatherData.Name,
                        Temperature = weatherData.Main.Temp,
                        Description = weatherData.Weather.FirstOrDefault()?.Description ?? "-",
                        // İkon kodunu al (örn: 04d)
                        IconCode = weatherData.Weather.FirstOrDefault()?.Icon ?? "01d"
                    };
                }
            }
        }
        catch
        {
            // Hata olursa (internet yoksa vs.) boş dönmesin, varsayılan dönelim
        }

        // Hata durumunda varsayılan veri
        return new WeatherViewModel
        {
            City = city,
            Temperature = 0,
            Description = "Veri Alınamadı",
            IconCode = "unknown"
        };
    }

    // --- JSON Karşılama Sınıfları (Helper Classes) ---
    // API'den dönen karmaşık JSON yapısını karşılamak için kullanılan yardımcı sınıflar
    private class WeatherResponse
    {
        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; }

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    private class WeatherInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    private class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }
    }
}