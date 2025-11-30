using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VeterinaryClinic.UI.Models.Auth;

namespace VeterinaryClinic.UI.Services
{
    public interface IAuthApiClient
    {
        Task<AuthResponseDto?> LoginAsync(string email, string password);
    } 
}

