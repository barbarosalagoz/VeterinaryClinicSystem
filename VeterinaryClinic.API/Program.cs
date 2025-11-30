using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VeterinaryClinic.API.Configuration;
using VeterinaryClinic.API.Infrastructure;
using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.Business.Services;
using VeterinaryClinic.DataAccess.Context;
using VeterinaryClinic.DataAccess.UnitOfWork;
using VeterinaryClinic.Messaging;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. CORS Ekleme (Failed to fetch hatasını çözer) =================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // localhost sorunlarını çözer
              .AllowCredentials());
});

// ================= JWT Ayarları =================
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
builder.Services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<JwtSettings>>().Value);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
// DÜZELTME 1: AuthController ile uyumlu olması için ASCII yerine UTF8 yapıldı.
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ================= MVC & Swagger =================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VeterinaryClinic.API", Version = "v1" });

    // DÜZELTME 2: Swagger Security Type 'Http' olarak değiştirildi.
    // Artık token'ı yapıştırırken başına 'Bearer ' yazmana gerek yok, Swagger bunu otomatik yapacak.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token'ınızı (başında Bearer OLMADAN) doğrudan aşağıya yapıştırın."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// ================= DbContext & Services =================
builder.Services.AddDbContext<VeterinaryClinicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// RabbitMQ
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();

var app = builder.Build();

// ================= Middleware =================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS Middleware'i HTTPS'den hemen sonra çağır
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();