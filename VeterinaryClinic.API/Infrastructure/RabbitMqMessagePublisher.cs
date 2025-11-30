using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using VeterinaryClinic.Messaging;

namespace VeterinaryClinic.API.Infrastructure
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly RabbitMqOptions _options;

        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqMessagePublisher(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        private async Task<IChannel> GetOrCreateChannelAsync()
        {
            if (_channel is not null)
                return _channel;

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            if (!string.IsNullOrWhiteSpace(_options.Exchange))
            {
                await _channel.ExchangeDeclareAsync(
                    exchange: _options.Exchange,
                    type: ExchangeType.Fanout,
                    durable: true,
                    autoDelete: false,
                    arguments: null);
            }

            return _channel;
        }

        public async Task PublishAsync<T>(string routingKey, T message)
        {
            var channel = await GetOrCreateChannelAsync();

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var exchange = string.IsNullOrWhiteSpace(_options.Exchange)
                ? ""                         // doğrudan kuyruğa publish
                : _options.Exchange;         // fanout / configured exchange

            // Yeni API'de BasicProperties sınıfını kullanıyoruz (IAmqpHeader implement ediyor)
            var props = new BasicProperties();

            await channel.BasicPublishAsync<BasicProperties>(
                exchange: exchange,
                routingKey: routingKey ?? string.Empty,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: CancellationToken.None);
        }
    }
}
