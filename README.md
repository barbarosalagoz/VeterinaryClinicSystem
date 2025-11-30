VeterinaryClinicSystem

EN: A realistic Veterinary Clinic Management System built with .NET 10, layered architecture, JWT auth, RabbitMQ messaging and a modern ASP.NET Core MVC UI.
TR: .NET 10, katmanlı mimari, JWT kimlik doğrulama, RabbitMQ mesajlaşma ve modern ASP.NET Core MVC arayüzü ile geliştirilmiş gerçekçi bir Veteriner Klinik Yönetim Sistemi.

Geliştirme amacı:

EN: Full-stack learning & portfolio project that follows real-world patterns

TR: Gerçek hayata yakın mimarilerle hazırlanmış bir full-stack öğrenme ve portföy projesi

Contents / İçindekiler

Overview / Genel Bakış

Architecture / Mimari

Solution Structure / Çözüm Yapısı

Features / Özellikler

Tech Stack / Teknolojiler

Getting Started / Başlangıç

Auth Flow / Kimlik Doğrulama Akışı

Roadmap / Yol Haritası

Author / Yazar

1. Overview / Genel Bakış

EN

Manage animals, owners, appointments, treatments and payments

API: ASP.NET Core Web API secured with JWT

UI: ASP.NET Core MVC with cookie authentication and Bootstrap 5 dashboard

Payments: Per-appointment payment summary (total cost / total paid / remaining balance)

Messaging: RabbitMQ integration for domain events (e.g. PaymentCreated)

Weather: External Weather API integration for dashboard info

TR

Hayvan, sahip, randevu, tedavi ve ödeme yönetimi

JWT ile korunan ASP.NET Core Web API

Cookie kimlik doğrulamalı ASP.NET Core MVC arayüzü (Bootstrap 5 dashboard)

Randevu bazlı ödeme özeti (toplam tutar / ödenen / kalan borç)

RabbitMQ ile domain event tabanlı mesajlaşma (örn. PaymentCreated)

Dashboard’da dış Weather API üzerinden hava durumu gösterimi

2. Architecture / Mimari

Layers / Katmanlar

Entities

DataAccess

Business

API

UI

Ek bileşenler:

Messaging (RabbitMQ publisher)

Worker (RabbitMQ consumer)

Akış:

UI → API → Business → DataAccess → Database

Business katmanı gerektiğinde Messaging katmanına mesaj yollar

Worker bu mesajları tüketerek loglama, raporlama vb. işler yapar

3. Solution Structure / Çözüm Yapısı

Solution: VeterinaryClinicSystem.sln

VeterinaryClinic.Entities

Domain entity’ler (User, Animal, Appointment, Treatment, Payment, WeatherInfo vb.)

Enum’lar (UserRole, AppointmentStatus, PaymentMethod, TreatmentType)

VeterinaryClinic.DataAccess

VeterinaryClinicDbContext (EF Core, SQL Server)

Generic repository & UnitOfWork altyapısı

VeterinaryClinic.Business

Service arayüzleri: IAnimalService, IAppointmentService, ITreatmentService, IPaymentService…

İş kuralları ve transaction yönetimi

VeterinaryClinic.API

ASP.NET Core Web API

JWT konfigürasyonu

Swagger + JWT security scheme

Auth, Animals, Appointments, Treatments, Payments controller’ları

RabbitMQ publisher ve Weather API entegrasyonu

VeterinaryClinic.UI

ASP.NET Core MVC

Modern dashboard layout (Bootstrap 5)

Modules: Dashboard, Animals, Appointments, Payments, Customers

Cookie auth + HttpClient + AuthenticatedHttpClientHandler

VeterinaryClinic.Messaging

IMessagePublisher arayüzü

RabbitMqOptions konfig sınıfı

VeterinaryClinic.Worker

RabbitMQ consumer / background worker

İleride loglama, raporlama vb. işler için altyapı

4. Features / Özellikler
Animals / Hayvanlar

CRUD işlemleri

Her hayvan bir owner (User – Customer) ile ilişkili

UI üzerinden listeme, detay ve düzenleme

Appointments / Randevular

Hayvan bazlı randevu oluşturma

Statüler: Scheduled, Completed, Cancelled

İlgili tedavileri ve ödemeleri bağlama

Treatments / Tedaviler

Randevuya bağlı tedavi / işlem ekleme

Tür (TreatmentType), açıklama ve ücret alanları

Payment summary hesaplamasına otomatik dahil olurlar

Payments / Ödemeler

Randevu bazlı ödeme özeti:

TotalTreatmentCost = tedavilerin toplamı

TotalPaid = ödemelerin toplamı

RemainingBalance = TotalTreatmentCost − TotalPaid

Ödeme geçmişi:

Tarih

Ödeme yöntemi (Cash, CreditCard, BankTransfer vb.)

Tutar

Ödeme sonrası isteğe bağlı RabbitMQ event’i (PaymentCreated) yayınlanabilir

Authentication / Kimlik Doğrulama

API (JWT)

AuthController register & login endpoint’leri

Şifreler eğitim amaçlı SHA256 hash ile saklanır

Login sonrasında AuthResponse DTO’su ile JWT döner

UI (Cookie + JWT)

/Account/Login ekranı

UI, API’ye login isteği gönderir (AuthApiClient)

Dönen token cookie içindeki access_token claim’inde tutulur

AuthenticatedHttpClientHandler, bu claim’i okuyup tüm API isteklerine
Authorization: Bearer <token> header’ını ekler

API tarafında [Authorize] attribute’u ile endpoint’ler korunur

Messaging (RabbitMQ)

RabbitMqMessagePublisher nesnesi kuyruklara mesaj yollar

Konfigürasyon appsettings.json içindeki RabbitMQ bölümünden alınır

VeterinaryClinic.Worker kuyruğu dinleyerek bu mesajları işler

Weather API

Harici Weather API ile şehir bazlı hava durumu bilgisi çekilir

Dashboard’daki kartlar bu veriyi kullanabilir

5. Tech Stack / Teknolojiler

.NET 10

ASP.NET Core Web API

ASP.NET Core MVC

Entity Framework Core (SQL Server)

JWT (System.IdentityModel.Tokens.Jwt)

RabbitMQ (RabbitMQ.Client)

Bootstrap 5 & Bootstrap Icons

HttpClient + DelegatingHandler

IOptions<T> ile configuration binding

6. Getting Started / Başlangıç
6.1. Repo’yu Klonla
git clone https://github.com/barbarosalagoz/VeterinaryClinicSystem.git
cd VeterinaryClinicSystem

6.2. API Ayarları (VeterinaryClinic.API/appsettings.json)

ConnectionStrings:DefaultConnection:

SQL Server connection string’in

Örn: Server=.;Database=VeterinaryClinicDb;Trusted_Connection=True;TrustServerCertificate=True

JwtSettings:

Issuer: VetClinic

Audience: VetClinicClient

SecretKey: en az 32 karakterlik rastgele string

AccessTokenExpirationMinutes: örneğin 60

RabbitMQ:

HostName: localhost

UserName: guest

Password: guest

Port: 5672

VirtualHost: /

WeatherApi:

BaseUrl: Örn. https://api.weatherapi.com

ApiKey: kendi anahtarın

6.3. Database Migration
cd VeterinaryClinic.API
dotnet ef migrations add InitialCreate -p ../VeterinaryClinic.DataAccess -s .
dotnet ef database update -p ../VeterinaryClinic.DataAccess -s .

6.4. API’yi Çalıştır
cd VeterinaryClinic.API
dotnet run


Swagger: https://localhost:<PORT>/swagger

6.5. UI’yi Çalıştır
cd VeterinaryClinic.UI
dotnet run


UI: https://localhost:<PORT>/

6.6. (Opsiyonel) Worker’ı Çalıştır
cd VeterinaryClinic.Worker
dotnet run

7. Auth Flow / Kimlik Doğrulama Akışı (Özet)

Kullanıcı /Account/Login ekranından giriş yapar.

UI, API’ye POST /api/Auth/login isteği gönderir.

API kullanıcıyı doğrular ve JWT içeren AuthResponse döner.

UI, token’ı cookie’de access_token claim’i olarak saklar.

AuthenticatedHttpClientHandler bu claim’i tüm API isteklerine Bearer token olarak ekler.

API tarafında [Authorize] attribute’lü endpoint’ler token’ı doğrular.

8. Roadmap / Yol Haritası

Owner / Customer yönetimi için detaylı ekranlar

RabbitMQ event’leri üzerinden gelişmiş raporlama

Tam EN / TR UI dil desteği

Business ve API katmanları için testler

Dashboard’da daha fazla metrik (ciro, ziyaret sayısı, tür dağılımı vb.)

9. Author / Yazar

Barbaros Emre Alagöz

GitHub: https://github.com/barbarosalagoz

EN: Mainly a learning and portfolio project, but it follows real-world patterns and is designed to be extensible and maintainable.
TR: Ağırlıklı olarak öğrenme ve portföy amacıyla geliştirilmiş olup, gerçek dünya desenlerine uygun, genişletilebilir ve sürdürülebilir bir yapıda tasarlanmıştır.
