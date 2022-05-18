using Jobsity.Chat.Api.Application.Chat;
using Jobsity.Chat.Api.MessageBroker.Abstractions.Producer;
using Jobsity.Chat.Api.MessageBroker.Producer;
using Jobsity.Chat.Api.Persistance;
using Jobsity.Chat.Api.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();
builder.Services.AddScoped<IChatProcessor, ChatProcessor>();
builder.Services.AddSingleton<IBotChatProcessor, BotChatProcessor>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

//Adding subscriber for the rabbitMQ
builder.Services.AddHostedService<RabbitMQSubscriber>();

//Adding cros policy for the web client
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("https://localhost:7175")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddDbContext<ChatContext>(opt => opt.UseInMemoryDatabase("ChatMessagesDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();
