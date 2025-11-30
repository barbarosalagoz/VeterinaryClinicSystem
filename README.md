🐾 Veterinary Clinic Management System (VetClinicSystem)

TR: .NET 10, katmanlı mimari, JWT kimlik doğrulama, RabbitMQ mesajlaşma ve modern ASP.NET Core MVC arayüzüyle geliştirilmiş gerçekçi bir Veteriner Klinik Yönetim Sistemi.
EN: A realistic, training-oriented Veterinary Clinic Management System built with .NET 10, layered architecture, JWT authentication, RabbitMQ messaging and a modern ASP.NET Core MVC UI.

✨ Developed as a full-stack case study inspired by Murat Yücedağ's trainings, then extended with real-world patterns (RabbitMQ, JWT, modern dashboard UI).

💡 Overview / Genel Bakış

VeterinaryClinicSystem, bir veteriner kliniğinin temel iş akışlarını yöneten, genişletilebilir ve sürdürülebilir olacak şekilde tasarlanmış bir projedir.

Ana Öğrenim Amaçları:

Katmanlı (N-Tier) ve Temiz Mimari Uygulamaları

EF Core, Generic Repository ve Unit of Work Desenleri

API-UI İletişimi (HttpClient ve Delegating Handlers)

JWT ve Cookie Tabanlı Kimlik Doğrulama Akışının Birleştirilmesi

RabbitMQ ile Asenkron Mesajlaşma (Worker Service)

🧱 Architecture / Mimari

Proje, katmanlar arası sorumlulukların net olduğu altı (6) ayrı projeye ayrılmıştır.

Proje Adı

Sorumluluk

Açıklama

VeterinaryClinic.Entities

Domain

Varlıklar (Animal, Appointment, Payment, Treatment vb.) ve Enum'lar.

VeterinaryClinic.DataAccess

DAL

EF Core DbContext, Generic Repository ve Unit of Work uygulaması.

VeterinaryClinic.Business

Servisler

İş kuralları, validasyonlar ve veritabanı işlemlerini koordine eden servisler.

VeterinaryClinic.API

API

Veri sunumu, JWT Auth, Swagger ve RabbitMQ mesaj yayınlama katmanı.

VeterinaryClinic.UI

Frontend

Modern Dashboard'lu ASP.NET Core MVC arayüzü. (Giriş/Kayıt, CRUD ekranları).

VeterinaryClinic.Worker

Consumer

RabbitMQ'dan gelen mesajları dinleyen ve işleyen arka plan servisi.

🛠️ Features / Özellikler

Hayvanlar, Randevular ve Tedaviler

Hayvanlar: CRUD operasyonları, hayvan sahibi (User) ile ilişkilendirme.

Randevular: Oluşturma, listeleme, iptal etme. Durum yönetimi (Scheduled/Completed/Cancelled).

Tedaviler: Randevuya birden fazla tedavi (Muayene, Aşı vb.) kalemi ekleme ve maliyet hesaplama.

💰 Ödemeler ve Finans

Ödeme Özeti: Randevu bazlı TotalTreatmentCost (Toplam Tedavi Tutarı), TotalPaid (Ödenen) ve RemainingBalance (Kalan Borç) hesaplamaları.

Ödeme Geçmişi: Yapılan ödemelerin (tutar, yöntem, tarih) listelenmesi.

🛡️ Güvenlik ve Kimlik Doğrulama

API: AuthController üzerinden JWT Token üretimi. (Eğitim amaçlı SHA256 şifreleme ile).

UI: Cookie Authentication. AuthenticatedHttpClientHandler kullanarak her API çağrısına JWT'nin otomatik olarak Authorization: Bearer <token> olarak eklenmesi.

⚙️ Entegrasyonlar

RabbitMQ: Ödeme ve önemli olaylar için asenkron mesaj yayınlama. Worker servisi bu mesajları tüketir (Örn: Fatura oluşturma simülasyonu).

Weather API: Dashboard'da anlık hava durumu bilgisi (Örn: Istanbul, 19°C) gösterimi.

🚀 Getting Started / Başlangıç

Bu projeyi çalıştırmak için Docker (RabbitMQ için) ve MS SQL Server gereklidir.

1️⃣ Projeyi İndirme (Clone)

git clone [https://github.com/barbarosalagoz/VeterinaryClinicSystem.git](https://github.com/barbarosalagoz/VeterinaryClinicSystem.git)
cd VeterinaryClinicSystem


2️⃣ RabbitMQ'yu Çalıştırma

Docker'da standart RabbitMQ konteynerini başlatın:

docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management


3️⃣ API Configuration (appsettings.json)

VeterinaryClinic.API/appsettings.json dosyasını açarak bağlantı ve RabbitMQ ayarlarınızı kontrol edin.

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=VeterinaryClinicDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Issuer": "VetClinic",
    "Audience": "VetClinicClient",
    "SecretKey": "CHANGE_ME_WITH_SAFE_KEY_32+_CHARS",
    "AccessTokenExpirationMinutes": 60
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest",
    "Port": 5672,
    "VirtualHost": "/"
  },
  "WeatherApi": {
    "BaseUrl": "[https://api.openweathermap.org](https://api.openweathermap.org)",
    "ApiKey": "YOUR_OPENWEATHERMAP_KEY"
  }
}


⚠️ Kendi API Key ve SecretKey değerlerinizi kullanın.

4️⃣ Veritabanı Geçişleri (Migrations)

VeterinaryClinic.API klasöründeyken migrations ve database update işlemlerini yapın:

# (İlk kez yapılıyorsa)
dotnet ef migrations add InitialCreate -p ../VeterinaryClinic.DataAccess -s .

# Veritabanını güncelle
dotnet ef database update -p ../VeterinaryClinic.DataAccess -s .


5️⃣ Projeleri Başlatma (Multi-Startup)

Visual Studio'da Solution'a sağ tıklayıp "Set Startup Projects..." seçeneğini kullanarak API, UI ve Worker projelerini aynı anda çalışacak şekilde ayarlayın ve başlatın.

6️⃣ Test (Auth & Worker)

Auth: UI arayüzünden yeni bir kullanıcı kaydedin (/Account/Register) ve giriş yapın.

Worker: UI'da bir ödeme işlemi yaptığınızda, Worker projesinin konsolunda "Fatura PDF'i oluşturuluyor" loglarını görmelisiniz.

🗺️ Roadmap / Yol Haritası

Projenin gelecekteki hedefleri ve geliştirmeye açık alanları:

Gelişmiş Raporlama: RabbitMQ mesajları üzerinden gerçek zamanlı dashboard metrikleri ve finansal raporlar.

Tedavi Geçmişi: Hayvan detay sayfasında tüm geçmiş tedavi ve aşı kayıtlarının listelenmesi.

UI/UX İyileştirmeleri: Tam EN/TR dil desteği ve daha detaylı owner (müşteri) yönetim ekranları.

Testler: Projeye Unit ve Integration testlerin eklenmesi.

👤 Author / Yazar

Barbaros Emre Alagöz

GitHub: @barbarosalagoz

LinkedIn: 

$$Profilinizi Buraya Ekleyin$$

Bu proje, gerçek dünya desenlerini öğrenmek ve uygulamak amacıyla geliştirilmiştir. Katkı ve geri bildirimleriniz değerlidir.
