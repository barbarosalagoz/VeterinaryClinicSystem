using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.DataAccess.Context;
using VeterinaryClinic.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // RabbitMQ ayarı

        services.Configure<RabbitMqOptions>(
            context.Configuration.GetSection("RabbitMq"));
      
        // DbContext

        services.AddDbContext<VeterinaryClinicDbContext>(options =>
        {
            var connStr = context.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connStr);
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
