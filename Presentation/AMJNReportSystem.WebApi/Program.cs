using AMJNReportSystem.Application;
using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Validators;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Gateway.Implementations;
using AMJNReportSystem.IOC.ServiceCollections;
using AMJNReportSystem.WebApi.Extensions;
using AMJNReportSystem.WebApi.HealthCheck;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

/*builder.AMJNReportSystem.WebApi.UseSerilog((_, config) =>
{
    config.WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration);
});*/

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddFluentValidators(builder.Configuration);
builder.Services.AddLocalization();
builder.Services.AddLogging();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddTransient<IGatewayHandler, GatewayHandler>();

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDatabase(connectionString);

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddCheck<HealthCheck>("HealthCheck");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization(); 

app.UseInfrastructure(builder.Configuration);

app.MapControllers(); 

app.Run();

