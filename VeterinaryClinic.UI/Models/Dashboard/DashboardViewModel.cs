using System.Collections.Generic;

namespace VeterinaryClinic.UI.Models
{
    public class DashboardViewModel
    {
        public int TotalAnimals { get; set; }
        public int TotalAppointments { get; set; }
        public decimal PendingPaymentsAmount { get; set; }
        public int ActiveVeterinarians { get; set; }

        // Son eklenen hayvanlar listesi
        public List<AnimalListItemViewModel> RecentAnimals { get; set; } = new List<AnimalListItemViewModel>();

        // Hava durumu özelliği (Soru işareti ile nullable yapıyoruz ki boş gelirse hata vermesin)
        public WeatherViewModel? Weather { get; set; }
    }
}