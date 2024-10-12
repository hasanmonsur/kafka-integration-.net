// See https://aka.ms/new-console-template for more information
using KafkaIntegrationConsumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
//builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
//builder.Services.AddSingleton<KafkaSettings>(sp =>
//{
//    return sp.GetRequiredService<IOptions<KafkaSettings>>().Value;
//});

builder.Services.AddHostedService<KafkaConsumerService>(); 

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.Run();