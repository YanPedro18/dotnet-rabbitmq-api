using Microsoft.AspNetCore.Mvc;
using WebApplication_rabbitmq.DTOs.RabbitMq;
using WebApplication_rabbitmq.Services;


namespace WebApplication_rabbitmq.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RabbitMqController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        public RabbitMqController(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] MessageRequest request)
        {
            await _rabbitMqService.SendMessageAsync(request.Message);
            return Ok("Mensagem enviada com sucesso!");
        }
    }
}
