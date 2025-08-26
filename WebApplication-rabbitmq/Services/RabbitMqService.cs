using RabbitMQ.Client;                // Biblioteca principal para se comunicar com RabbitMQ
using System.Text;                     // Para conversão de string <-> bytes
using System.Threading.Tasks;          // Para métodos assíncronos (async/await)
using WebApplication_rabbitmq.Services; // Para usar a interface IRabbitMqService

/// <summary>
/// Serviço responsável por enviar mensagens para uma fila RabbitMQ de forma assíncrona.
/// </summary>
public class RabbitMqService : IRabbitMqService
{
    // Configurações de conexão
    private readonly string _hostname;   // Endereço do servidor RabbitMQ
    private readonly string _username;   // Usuário do RabbitMQ
    private readonly string _password;   // Senha do usuário
    private readonly string _queueName;  // Nome da fila que receberá as mensagens

    /// <summary>
    /// Construtor que recebe as configurações do appsettings.json
    /// </summary>
    /// <param name="config">IConfiguration para ler as variáveis do appsettings.json</param>
    public RabbitMqService(IConfiguration config)
    {
        _hostname = config["RabbitMQ:HostName"];
        _username = config["RabbitMQ:UserName"];
        _password = config["RabbitMQ:Password"];
        _queueName = config["RabbitMQ:QueueName"];
    }

    /// <summary>
    /// Envia uma mensagem para a fila RabbitMQ de forma assíncrona.
    /// </summary>
    /// <param name="message">Mensagem que será enviada</param>
    public async Task SendMessageAsync(string message)
    {
        // Cria uma fábrica de conexões com os parâmetros do RabbitMQ
        var factory = new ConnectionFactory()
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };

        // Cria uma conexão assíncrona com o RabbitMQ
        // 'using' garante que a conexão será fechada ao final do bloco
        using var connection = await factory.CreateConnectionAsync();

        // Cria um canal assíncrono dentro da conexão
        using var channel = await connection.CreateChannelAsync();

        // Garante que a fila existe antes de enviar a mensagem
        // durable: false -> a fila não persiste após reinício do RabbitMQ
        // exclusive: false -> a fila não é exclusiva para esta conexão
        // autoDelete: false -> a fila não é deletada quando não há consumidores
        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        // Converte a mensagem de string para byte[]
        var body = Encoding.UTF8.GetBytes(message);

        // Publica a mensagem na fila
        // exchange: string.Empty -> usa a fila padrão
        // routingKey: nome da fila
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _queueName,
            body: body
        );

        // Exibe no console que a mensagem foi enviada
        Console.WriteLine($"Mensagem enviada: {message}");
    }
}
