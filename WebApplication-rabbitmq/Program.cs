using WebApplication_rabbitmq.Services;

var builder = WebApplication.CreateBuilder(args);

// ============================
// 1. Registrando servi�os
// ============================

// Registra o RabbitMqConsumer como HostedService (servi�o de background)
// Assim o consumidor inicia automaticamente quando a aplica��o inicia
builder.Services.AddHostedService<RabbitMqConsumer>();

// Registra o RabbitMqService para enviar mensagens (via Controller)
// Singleton � ok, pois mant�m a mesma inst�ncia durante toda a aplica��o
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

// Registra os Controllers da aplica��o
builder.Services.AddControllers();

// Adiciona o Swagger/OpenAPI para documenta��o e testes da API
builder.Services.AddSwaggerGen();

// Configura��o de CORS (libera requisi��es de qualquer origem, cabe�alho e m�todo)
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
// 2. Configura��o do pipeline HTTP
// ============================

if (app.Environment.IsDevelopment())
{
    // Habilita middleware do Swagger (documenta��o da API)
    app.UseSwagger();

    // Configura a interface SwaggerUI
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication-RabbitMQ v1");
        options.RoutePrefix = string.Empty; // Swagger na raiz da aplica��o
    });
}

// Aplica a pol�tica CORS
app.UseCors();

// Redireciona HTTP para HTTPS
app.UseHttpsRedirection();

// Middleware de autoriza��o (para [Authorize] nos controllers)
app.UseAuthorization();

// Mapeia os endpoints dos controllers
app.MapControllers();

// ============================
// 3. Inicia a aplica��o
// ============================
app.Run();
