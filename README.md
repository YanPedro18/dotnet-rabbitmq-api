📌 Projeto: API .NET 9 com RabbitMQ (Produtor e Consumidor Assíncrono)

Este projeto é uma API em .NET 9 que demonstra a integração com o RabbitMQ para publicação e consumo de mensagens.
A API possui um endpoint REST para enviar mensagens para uma fila, enquanto o consumidor roda em background processando as mensagens recebidas.

O projeto foi estruturado para rodar dentro de containers Docker (tanto a API quanto o RabbitMQ).

🚀 Tecnologias utilizadas

.NET 9

RabbitMQ.Client 7+

Swagger / OpenAPI

Docker

📂 Estrutura do projeto
📦 dotnet-rabbitmq-api
 ┣ 📂 DTOs
 ┃ ┗ 📄 MessageRequest.cs       # DTO para envio de mensagens
 ┣ 📂 Services
 ┃ ┣ 📄 RabbitMqService.cs  # Produtor: envia mensagens para a fila
 ┃ ┗ 📄 RabbitMqConsumer.cs # Consumidor: roda em background e lê a fila
 ┣ 📂 Interfaces
 ┃ ┗ 📄 IRabbitMqService.cs # Interface para abstração do produtor
 ┣ 📄 Controllers
 ┃ ┗ 📄 RabbitMqController.cs # Endpoint REST (api/rabbitmq)
 ┣ 📄 Program.cs             # Configuração principal da API
 ┣ 📄 Dockerfile             # Configuração para rodar a API em container
 ┗ 📄 README.md              # Este arquivo

⚙️ Funcionalidades

✅ Produtor (Publisher)

Endpoint POST /api/rabbitmq que recebe uma mensagem via JSON e publica na fila.

✅ Consumidor (Consumer)

Serviço em BackgroundService que escuta continuamente a fila.

Processa mensagens recebidas e exibe no console (Mensagem recebida: ...).

✅ Swagger UI

Disponível em:

http://localhost:<porta-da-api>/index.html


✅ Execução em Docker

API roda em container .NET 9

RabbitMQ roda em container próprio

🐳 Como rodar com Docker

Subir RabbitMQ (via Docker Hub):

docker run -d --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management


5672 → porta padrão para conexões da API

15672 → painel de administração do RabbitMQ

Painel disponível em: http://localhost:15672

Usuário padrão: guest

Senha padrão: guest

Rodar a API com Dockerfile:

docker build -t dotnet-rabbitmq-api .
docker run -d -p 32772:8080 --name api-rabbit dotnet-rabbitmq-api


Acessar Swagger:

http://localhost:32772/index.html

📡 Testando o fluxo
1. Enviar mensagem via endpoint

Endpoint: POST /api/rabbitmq
Exemplo de payload:

{
  "message": "teste enviando mensagem pra fila do rabbit mq"
}


Resposta:

{
  "success": true,
  "message": "Mensagem publicada com sucesso no RabbitMQ"
}

2. Ver mensagem consumida

No terminal da API (consumidor):

Mensagem recebida: teste enviando mensagem pra fila do rabbit mq

📋 Requisitos atendidos

✅ API em .NET 9

✅ Estrutura organizada com DTOs, Services e Interfaces

✅ Produtor envia mensagens assincronamente

✅ Consumidor roda em BackgroundService de forma assíncrona

✅ Documentação automática com Swagger

✅ Suporte a CORS

✅ Configuração para rodar em Docker

✅ RabbitMQ rodando em container separado

✅ Endpoint api/rabbitmq funcional

📜 Próximos passos (possíveis evoluções)

Salvar mensagens consumidas em banco de dados (ex: PostgreSQL, MongoDB).

Implementar reprocessamento em caso de falha.

Criar fila Dead Letter para mensagens não processadas.

Adicionar testes unitários e de integração.

👨‍💻 Autor

Projeto desenvolvido como estudo prático de RabbitMQ + .NET 9 + Docker.
