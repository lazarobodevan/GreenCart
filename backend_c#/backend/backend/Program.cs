using backend.Contexts;
using backend.Repositories;
using backend.Utils.Errors;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Environment.IsStaging()) {
    Console.WriteLine("Running in staging modeaaaa");
    Console.WriteLine(builder.Configuration.GetValue<String>("ConnectionStrings:DefaultConnection"));
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:DefaultConnection"));
    });
}

if (builder.Environment.IsDevelopment()) {
    Console.WriteLine("Running in development mode");
    Console.WriteLine(builder.Configuration.GetValue<String>("ConnectionStrings:Test"));
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:Dev"));
    });
}
if(builder.Environment.IsProduction()) {
    Console.WriteLine("Running in production mode");
    Console.WriteLine(builder.Configuration.GetValue<String>("ConnectionStrings:Dev"));
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:Prd"));
    });
}



builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProducerRepository, ProducerRepository>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
