namespace VeterinaryClinic.Messaging;


// Soyut mesaj yayınlayıcı: RabbitMQ, Azure Service Bus, AWS SQS gibi farklı altyapılar için ortak arayüz
public interface IMessagePublisher
{
    Task PublishAsync<T>(string topic, T message);
}
