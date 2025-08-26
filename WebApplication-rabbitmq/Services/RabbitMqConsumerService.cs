using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Classe responsável por consumir mensagens de uma fila RabbitMQ de forma assíncrona como serviço de background.
/// </summary>
public class RabbitMqConsumer : BackgroundService
{
    private readonly string _hostname;
    private readonly string _username;
    private readonly string _password;
    private readonly string _queueName;

    public RabbitMqConsumer(IConfiguration config)
    {
        _hostname = config["RabbitMQ:HostName"];
        _username = config["RabbitMQ:UserName"];
        _password = config["RabbitMQ:Password"];
        _queueName = config["RabbitMQ:QueueName"];
    }

    /// <summary>
    /// Método que o host chama automaticamente quando o serviço é iniciado.
    /// </summary>
    /// <param name="stoppingToken">Token que indica quando a aplicação está encerrando.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(_queueName, durable: false, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine($"Mensagem recebida: {message}");

            await Task.Yield(); // simula processamento assíncrono
        };

        await channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);

        // Mantém o serviço ativo enquanto a aplicação estiver rodando
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken); // espera 1s antes de checar novamente
        }
    }
}
