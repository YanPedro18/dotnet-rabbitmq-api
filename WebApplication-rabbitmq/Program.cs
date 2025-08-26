using WebApplication_rabbitmq.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================
// 1. Registrando serviços
// ============================

// Registra o RabbitMqConsumer como HostedService (serviço de background)
// Assim o consumidor inicia automaticamente quando a aplicação inicia
builder.Services.AddHostedService<RabbitMqConsumer>();

// Registra o RabbitMqService para enviar mensagens (via Controller)
// Singleton é ok, pois mantém a mesma instância durante toda a aplicação
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

// Registra os Controllers da aplicação
builder.Services.AddControllers();

// Adiciona o Swagger/OpenAPI para documentação e testes da API
builder.Services.AddSwaggerGen();

// Configuração de CORS (libera requisições de qualquer origem, cabeçalho e método)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ============================
// 2. Configuração do pipeline HTTP
// ============================

if (app.Environment.IsDevelopment())
{
    // Habilita middleware do Swagger (documentação da API)
    app.UseSwagger();

    // Configura a interface SwaggerUI
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication-RabbitMQ v1");
        options.RoutePrefix = string.Empty; // Swagger na raiz da aplicação
    });
}

// Aplica a política CORS
app.UseCors();

// Redireciona HTTP para HTTPS
app.UseHttpsRedirection();

// Middleware de autorização (para [Authorize] nos controllers)
app.UseAuthorization();

// Mapeia os endpoints dos controllers
app.MapControllers();

// ============================
// 3. Inicia a aplicação
// ============================
app.Run();
