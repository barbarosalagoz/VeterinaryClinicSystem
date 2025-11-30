using VeterinaryClinic.Messaging;

namespace VeterinaryClinic.API.Infrastructure;


// Şimdilik hiçbir şey yapmayan, sadec metodu başarıyla dönen publisher
public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string topic, T message)
    {
        // Burada log da yazabilirim ama şimdilik boş
        Console.WriteLine($"[NoOpPublisher] Topic: {topic}, Message Type: {typeof(T).Name}");
        return Task.CompletedTask;
    }
}
