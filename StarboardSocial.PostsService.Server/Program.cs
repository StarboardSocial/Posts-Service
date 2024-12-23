using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using RabbitMQ.Client;
using StarboardSocial.PostsService.Domain.Services;
using StarboardSocial.UserService.Data.Repositories;
using StarboardSocial.UserService.Domain.DataInterfaces;
using StarboardSocial.UserService.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

// RabbitMQ Config
try
{
    ConnectionFactory factory = new()
    {
        UserName = builder.Configuration["Rabbit:UserName"]!,
        Password = builder.Configuration["Rabbit:Password"]!,
        VirtualHost = builder.Configuration["Rabbit:VirtualHost"]!,
        HostName = builder.Configuration["Rabbit:HostName"]!,
        Port = int.Parse(builder.Configuration["Rabbit:Port"]!)
    };

    IConnection conn = await factory.CreateConnectionAsync();
    IChannel channel = await conn.CreateChannelAsync();

    builder.Services.AddSingleton(channel);
    
    // Data Deletion
    builder.Services.AddScoped<IDataDeletionRepository, DataDeletionRepository>();
    builder.Services.AddScoped<IDataDeletionService, DataDeletionService>();
    builder.Services.AddScoped<DataDeletionConsumer>();
    builder.Services.AddHostedService<DataDeletionListener>();
    builder.Services.AddScoped<DataDeletionHandler>();
    
} catch (Exception e)
{
    Console.WriteLine("Error connecting to RabbitMQ");
    Console.WriteLine(e.Message);
}

// Database
IMongoClient mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDB")!);
builder.Services.AddSingleton<IMongoDatabase>(_ => mongoClient.GetDatabase(builder.Configuration["MongoDB:DatabaseName"]!));

// Services
builder.Services.AddScoped<IPrivateService, PrivateService>();
builder.Services.AddScoped<IPublicService, PublicService>();
builder.Services.AddScoped<IPrivateRepository, PrivateRepository>();
builder.Services.AddScoped<IPublicRepository, PublicRepository>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/post");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();