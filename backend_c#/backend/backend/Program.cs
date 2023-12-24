using System;
using backend.Contexts;
using backend.Producer.Repository;
using backend.Product.Repository;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Environment.IsStaging()){
    Console.WriteLine("Running in staging mode");
    Console.WriteLine(builder.Configuration.GetValue<String>("ConnectionStrings:DefaultConnection"));
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:DefaultConnection"));
        options.UseExceptionProcessor();
    });
}

if (builder.Environment.IsDevelopment()){
    Console.WriteLine("Running in development mode");
    Console.WriteLine(builder.Configuration.GetValue<String>("ConnectionStrings:Test"));
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:Dev"));
        options.UseExceptionProcessor();
    });
}

if (builder.Environment.IsProduction()){
    Console.WriteLine("Running in production mode");
    Console.WriteLine(builder.Configuration.GetValue<String>("ConnectionStrings:Dev"));
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:Prd"));
        options.UseExceptionProcessor();
    });
}


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProducerRepository, ProducerRepository>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();