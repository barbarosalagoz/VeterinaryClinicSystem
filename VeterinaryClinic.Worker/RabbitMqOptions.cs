namespace VeterinaryClinic.Worker;

public class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string Exchange { get; set; } = "veterinaryclinic.appointments";
    public string QueueName { get; set; } = "vet.appointments.log";
}
