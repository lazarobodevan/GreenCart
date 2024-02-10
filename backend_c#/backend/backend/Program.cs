using System;
using System.Globalization;
using System.IO;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using backend.Contexts;
using backend.Producer.Repository;
using backend.Producer.Services;
using backend.ProducerPicture.Services;
using backend.Product.Enums;
using backend.Product.Repository;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional:true).Build();

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
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetValue<String>("ConnectionStrings:Dev"));
    dataSourceBuilder.UseNetTopologySuite();
    var dataSource = dataSourceBuilder.Build();
    builder.Services.AddDbContext<DatabaseContext>(options => {
        
        options.UseNpgsql(dataSource, o => o.UseNetTopologySuite());
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

builder.Services.AddScoped<IAmazonS3>(provider => {
    var awsCredentials = new BasicAWSCredentials(
        builder.Configuration.GetValue<string>("AmazonS3:AccessKeyId"),
        builder.Configuration.GetValue<string>("AmazonS3:SecretKey")
    );

    var awsRegion = RegionEndpoint.SAEast1;

    return new AmazonS3Client(awsCredentials, awsRegion);
});

// Registra o PictureService
builder.Services.AddScoped<IProductPictureService, ProductPictureService>(provider => {
    var amazonS3 = provider.GetRequiredService<IAmazonS3>();

    return new ProductPictureService(
        amazonS3,
        builder.Configuration.GetValue<string>("AmazonS3:BucketName")!
    );
});

builder.Services.AddScoped<IProducerPictureService, ProducerPictureService>(provider => {
    var amazonS3 = provider.GetRequiredService<IAmazonS3>();

    return new ProducerPictureService(
        amazonS3,
        builder.Configuration.GetValue<string>("AmazonS3:BucketName")!
    );
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProducerRepository, ProducerRepository>();
CultureInfo.CurrentCulture = new CultureInfo("en-US");

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