using VeterinaryClinic.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IAppointmentApiClient, AppointmentApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ApiBaseUrl"]!);
});
builder.Services.AddHttpClient<IPaymentApiClient, PaymentApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ApiBaseUrl"]!);
});
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();

// Tedavi servisini sisteme tanıtıyoruz
builder.Services.AddHttpClient<ITreatmentApiClient, TreatmentApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ApiBaseUrl"]!);
}).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthenticatedHttpClientHandler>();

var apiBaseUrl = builder.Configuration["ApiSettings:ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiSettings:ApiBaseUrl config key is missing.");

builder.Services.AddHttpClient<IAnimalApiClient, AnimalApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

builder.Services.AddHttpClient<IAppointmentApiClient, AppointmentApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

builder.Services.AddHttpClient<IPaymentApiClient, PaymentApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

// Auth (login) için handler yok, çünkü login çağrısı token olmadan yapılacak
builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
