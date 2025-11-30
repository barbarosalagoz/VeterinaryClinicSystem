using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VeterinaryClinic.DataAccess.Context;
using VeterinaryClinic.Entities;
using VeterinaryClinic.Worker.Events;

namespace VeterinaryClinic.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly RabbitMqOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(
        ILogger<Worker> logger, 
        IOptions<RabbitMqOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _options = options.Value;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Appointment Worker starting...");

        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            Port = _options.Port,
            VirtualHost = _options.VirtualHost,
            //DispatchConsumersAsync = true
        };

        await using var connection =
            await factory.CreateConnectionAsync("vet-worker", stoppingToken);

        await using var channel =
            await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        // Queue ve binding zaten mevcut ama idempotent olsun diye tekrar tanımlıyorum

        await channel.QueueDeclareAsync(
            queue: _options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
            );

        await channel.QueueBindAsync(
            queue: _options.QueueName,
            exchange: _options.Exchange,
            routingKey: string.Empty,
            cancellationToken: stoppingToken
            );

        // Prefetch ayarı (tek seferde tek mesaj)
        await channel.BasicQosAsync(0, prefetchCount: 1, global: false,
            cancellationToken: stoppingToken
            );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());

            _logger.LogInformation("📥 Appointment event received: {Json}", json);

            AppointmentCreatedEvent? evt = null;

            try
            {
                evt = JsonSerializer.Deserialize<AppointmentCreatedEvent>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event couldn't be deserialized.");
            }

            if (evt != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VeterinaryClinicDbContext>();

                var log = new AppointmentLog
                {
                    AppointmentId = evt.AppointmentId,
                    AnimalId = evt.AnimalId,
                    OccuredAt = DateTime.UtcNow,
                    OwnerFullName = evt.OwnerFullName,
                    OwnerEmail = evt.OwnerEmail,
                    PayloadJson = json
                };

                db.AppointmentLogs.Add(log);
                await db.SaveChangesAsync(stoppingToken);
            }

            await channel.BasicAckAsync(
                deliveryTag: ea.DeliveryTag,
                multiple: false,
                cancellationToken: stoppingToken
                );
        };

        await channel.BasicConsumeAsync(
            queue: _options.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
            );

        _logger.LogInformation("Worker is now listening on queue {Queue}", _options.QueueName);

        // Worker ayakta kalsın
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }


    }


}

