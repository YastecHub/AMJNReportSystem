using AMJNReportSystem.Application;
using AMJNReportSystem.Application.Services;
using AMJNReportSystem.Gateway.Implementations;
using AMJNReportSystem.IOC.ServiceCollections;
using AMJNReportSystem.WebApi.Extensions;
using AMJNReportSystem.WebApi.HealthCheck;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

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

builder.Services.AddCors(options => options.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDatabase(connectionString);

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddCheck<HealthCheck>("HealthCheck");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();


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

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseInfrastructure(builder.Configuration);
app.MapControllers();
app.Run();

