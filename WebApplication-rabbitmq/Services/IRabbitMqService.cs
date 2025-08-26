namespace WebApplication_rabbitmq.Services
{
    public interface IRabbitMqService
    {
        Task SendMessageAsync(string message);
    }
}
