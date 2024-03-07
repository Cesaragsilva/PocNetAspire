using NetAspireApp.ApiService;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddRabbitMQ("rabbitmessage");

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddHttpClient<ExternalApiClient>(client => client.BaseAddress = new("https://external-api"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/post-message", (IConnection connection) =>
{
    try {
        var queueName = "test-queue";
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queueName, exclusive: false, durable: true);
        channel.BasicPublish(exchange: "", queueName, null, body: JsonSerializer.SerializeToUtf8Bytes(new { Title = "Book01", Description = "Description of Book01" }));

        return $"A Message was sent to RabbitMQ Queue";
    }
    catch(Exception ex){
        return $"An error occurred to send a message to RabbitMQ - {ex.Message}";
    }
});

app.MapGet("/send-hit", async (ExternalApiClient externalApi) =>
{
    try
    {
        await externalApi.SendHit("Text received from .NET Aspire ApiService");
        return "A hit was sent to external API";
    }
    catch (Exception ex) {
        return $"An error occurred during request external API: {ex?.Message}";
    }
});


app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
