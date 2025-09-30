using Consumer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddControllers();
builder.Services.AddSingleton<RabbitMqConsumerService>();
builder.Services.AddHostedService<RabbitMqConsumerService>(sp => sp.GetRequiredService<RabbitMqConsumerService>());

var app = builder.Build();

app.MapControllers();

app.Run();
