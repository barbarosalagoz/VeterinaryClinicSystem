using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.DataAccess.Context;

public class VeterinaryClinicDbContext : DbContext
{
    public VeterinaryClinicDbContext(DbContextOptions<VeterinaryClinicDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Treatment> Treatments => Set<Treatment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<WeatherInfo> WeatherInfos => Set<WeatherInfo>();
    public DbSet<AppointmentLog> AppointmentLogs { get; set; } = null!;
    public DbSet<ReportLog> ReportLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeterinaryClinicDbContext).Assembly);
    }
}