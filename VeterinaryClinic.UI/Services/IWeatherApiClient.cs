using VeterinaryClinic.UI.Models;

namespace VeterinaryClinic.UI.Services;

public interface IWeatherApiClient
{
    // Şehir ismine göre hava durumunu getiren metodun tanımı
    Task<WeatherViewModel> GetWeatherAsync(string city);
}
