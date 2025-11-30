using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace VeterinaryClinic.UI.Services;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Metodu 'async' yaptık çünkü GetTokenAsync asenkron çalışır
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // DÜZELTME: Token'ı Claim yerine, AuthenticationProperties'den (standart yerinden) okuyoruz.
        // Bu metodun çalışması için Login olurken "authProperties.StoreTokens" kullanılmış olmalı.
        var accessToken = await httpContext.GetTokenAsync("access_token");

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}