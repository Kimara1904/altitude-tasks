using Exceptions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Task1.CacheSystem;
using Task1.CacheSystem.Interfaces;
using Task1.FileSystem;
using Task1.FileSystem.Interfaces;
using Task1.Services;
using Task1.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Task4 UseCase API",
        Description = "An ASP.NET Core Web API for managing Task4 UseCase items",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddStackExchangeRedisCache(options
    => options.Configuration = builder.Configuration.GetConnectionString("RedisCache"));

builder.Services.AddScoped<IFileSystem, FileSystem>();
builder.Services.AddScoped<ICacheSystem, CacheSystem>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<ExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.Run();
