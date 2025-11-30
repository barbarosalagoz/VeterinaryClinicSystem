🐾 VeterinaryClinicSystem

EN: A realistic Veterinary Clinic Management System built with .NET 10, layered architecture, JWT auth, RabbitMQ messaging and a modern ASP.NET Core MVC UI.
TR: .NET 10, katmanlı mimari, JWT kimlik doğrulama, RabbitMQ mesajlaşma ve modern ASP.NET Core MVC arayüzüyle geliştirilmiş gerçekçi bir Veteriner Klinik Yönetim Sistemi.

Developed as a full-stack case study inspired by Murat Yücedağ’s trainings, then extended with real-world patterns (RabbitMQ, JWT, modern dashboard UI).
Murat Yücedağ’ın eğitimlerinden esinlenilen bir full-stack case çalışmasıdır; RabbitMQ, JWT ve modern dashboard arayüzü ile gerçek hayata daha yakın hale getirilmiştir.

📌 Overview / Genel Bakış

EN:

Manage animals, owners, appointments, treatments and payments

API: ASP.NET Core Web API secured with JWT

UI: ASP.NET Core MVC with cookie authentication and a Bootstrap 5 dashboard

Payments: Per-appointment payment summary (total cost / total paid / remaining balance)

Messaging: RabbitMQ integration for events (e.g. PaymentCreated)

Weather: External Weather API integration for contextual dashboard info

TR:

Hayvan, sahip, randevu, tedavi ve ödeme yönetimi

JWT ile korunan ASP.NET Core Web API

Cookie kimlik doğrulamalı ASP.NET Core MVC arayüzü (Bootstrap 5 dashboard)

Randevu bazlı ödeme özeti (toplam tutar / ödenen / kalan borç)

RabbitMQ üzerinden mesajlaşma (ör. PaymentCreated olayı)

Dashboard’da kullanılan dış Weather API entegrasyonu

🧱 Architecture / Mimari

Layered Architecture / Katmanlı Mimari:

Entities

DataAccess

Business

API

UI

Ek olarak:

Messaging (RabbitMQ)

Worker (Consumer)

Veri akışı kabaca şöyle:

UI → API → Business → DataAccess → Database

Business gerektiğinde Messaging katmanına mesaj gönderir

Worker bu mesajları tüketerek loglama, raporlama vb. görevleri yapar

📂 Solution Structure / Çözüm Yapısı

Solution: VeterinaryClinicSystem.sln

VeterinaryClinic.Entities – Domain entity ve enum’lar

VeterinaryClinic.DataAccess – EF Core DbContext, repository, UnitOfWork

VeterinaryClinic.Business – İş kuralları ve servisler

VeterinaryClinic.API – Web API (JWT, Swagger, RabbitMQ, Weather)

VeterinaryClinic.UI – ASP.NET Core MVC UI (Dashboard, Animals, Appointments, Payments)

VeterinaryClinic.Messaging – IMessagePublisher arayüzü, RabbitMqOptions vb.

VeterinaryClinic.Worker – RabbitMQ tüketicisi / arka plan worker uygulaması

🧾 Features / Özellikler
Animals / Hayvanlar

Hayvanlar için CRUD

Her hayvan bir sahibiyle (User – Customer) ilişkilidir

Appointments / Randevular

Belirli hayvan için randevu oluşturma

Durumlar: Scheduled, Completed, Cancelled

UI üzerinden listeleme, filtreleme, yönetim

Treatments / Tedaviler

Randevuya bağlı tedavi / işlem ekleme

TreatmentType, açıklama ve Cost alanları

Ödeme özetine otomatik dahil olurlar

Payments / Ödemeler

Randevu bazlı ödeme özeti:

TotalTreatmentCost = tedavilerin toplamı

TotalPaid = ödemelerin toplamı

RemainingBalance = TotalTreatmentCost − TotalPaid

Ödeme geçmişi listesi:

Tarih

Yöntem (Cash, CreditCard, BankTransfer vb.)

Tutar

Ödeme alındıktan sonra RabbitMQ’ye event gönderilebilir (örn. PaymentCreated)

Authentication / Kimlik Doğrulama

API tarafı (JWT):

AuthController üzerinden register ve login

Şifreler eğitim amaçlı olarak SHA256 hash ile saklanır

Başarılı login sonrası JWT üretilir ve AuthResponse DTO’su olarak döner

UI tarafı (cookie + JWT):

/Account/Login sayfası üzerinden giriş

UI, API’ye AuthApiClient ile login isteği gönderir

Dönen JWT, cookie içindeki access_token claim’i olarak saklanır

AuthenticatedHttpClientHandler bu claim’i okuyup tüm API isteklerine Authorization: Bearer <token> header’ını ekler

API tarafında [Authorize] attribute’lü endpoint’ler JWT’yi doğrular

Kısaca: UI cookie ile oturumu yönetir, API ise JWT ile endpoint’leri korur.

Messaging (RabbitMQ) / Mesajlaşma

RabbitMqMessagePublisher önemli olaylarda RabbitMQ kuyruğuna mesaj yollar

Ayarlar appsettings.json altındaki RabbitMQ bölümünden gelir

VeterinaryClinic.Worker uygulaması kuyruğu dinleyip bu mesajları tüketir

Loglama, rapor tablosu doldurma, e-posta tetikleme gibi işlere temel oluşturur

Weather API

Dış bir Weather API’den hava durumu verisi çekilir

Dashboard’da klinik lokasyonu için mevcut hava durumu gösterilebilir

🛠 Tech Stack / Teknolojiler

.NET 10

ASP.NET Core Web API

ASP.NET Core MVC

Entity Framework Core (SQL Server)

JWT (System.IdentityModel.Tokens.Jwt)

RabbitMQ (RabbitMQ.Client)

Bootstrap 5 & Bootstrap Icons

HttpClient + DelegatingHandler

IOptions<T> ile configuration binding

🚀 Getting Started / Başlangıç
1. Repository’yi klonla

git clone https://github.com/barbarosalagoz/VeterinaryClinicSystem.git

cd VeterinaryClinicSystem

2. API konfigürasyonu (VeterinaryClinic.API/appsettings.json)

ConnectionStrings:DefaultConnection:

SQL Server bağlantı cümleni buraya yaz:

Örnek: Server=.;Database=VeterinaryClinicDb;Trusted_Connection=True;TrustServerCertificate=True

JwtSettings:

Issuer: VetClinic

Audience: VetClinicClient

SecretKey: en az 32 karakterli rastgele bir metin

AccessTokenExpirationMinutes: örneğin 60

RabbitMQ:

HostName: localhost

UserName: guest

Password: guest

Port: 5672

VirtualHost: /

WeatherApi:

BaseUrl: örneğin https://api.weatherapi.com

ApiKey: kendi API anahtarın

Gerçek secret değerlerini public repo’ya koyma; geliştirme için appsettings.Development.json veya User Secrets kullanabilirsin.

3. Database migration

VeterinaryClinic.API klasörüne geç:

cd VeterinaryClinic.API

Gerekirse migration oluştur:

dotnet ef migrations add InitialCreate -p ../VeterinaryClinic.DataAccess -s .

Veritabanını oluştur/güncelle:

dotnet ef database update -p ../VeterinaryClinic.DataAccess -s .

4. API’yi çalıştır

cd VeterinaryClinic.API

dotnet run

Swagger UI:

https://localhost:<PORT>/swagger

5. UI’yi çalıştır

Yeni bir terminalde:

cd VeterinaryClinic.UI

dotnet run

MVC UI:

https://localhost:<PORT>/

6. (İsteğe bağlı) Worker’ı çalıştır

cd VeterinaryClinic.Worker

dotnet run

🔐 Auth Flow / Kimlik Doğrulama Akışı (Özet)

Kullanıcı /Account/Login sayfasından giriş yapar.

UI, API’ye POST /api/Auth/login isteği gönderir.

API, kullanıcıyı doğrular ve JWT içeren AuthResponse döner.

UI, token’ı cookie’ye access_token claim’i olarak yazar.

AuthenticatedHttpClientHandler bu claim’i okuyup tüm API çağrılarına Authorization: Bearer <token> ekler.

API, [Authorize] endpoint’lerinde JWT’yi doğrular.

🗺 Roadmap / Yol Haritası

Detaylı müşteri (owner) yönetim ekranları

RabbitMQ event’leri üzerinden gelişmiş raporlama

Tam EN / TR UI dil desteği

Business ve API için unit ve integration testleri

Dashboard’da daha fazla metrik (günlük ciro, ziyaret sayısı, tür dağılımı vb.)

👤 Author / Yazar

Barbaros Emre Alagöz

GitHub: https://github.com/barbarosalagoz

EN: This project is primarily for learning and portfolio purposes, but it follows real-world patterns and is designed to be extensible and maintainable.
TR: Bu proje ağırlıklı olarak öğrenme ve portföy amacıyla geliştirilmiştir; ancak gerçek dünya desenlerine uygun, genişletilebilir ve sürdürülebilir olacak şekilde tasarlanmıştır.
