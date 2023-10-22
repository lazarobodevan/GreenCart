using backend.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if (builder.Environment.IsDevelopment()) {
    Console.WriteLine("Runnin in development mode");
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:Dev"));
    });
}
if(builder.Environment.IsProduction()) {
    Console.WriteLine("Running in production mode");
    builder.Services.AddDbContext<DatabaseContext>(options => {
        options.UseNpgsql(builder.Configuration.GetValue<String>("ConnectionStrings:Prd"));
    });
}

var app = builder.Build();

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
