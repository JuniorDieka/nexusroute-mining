using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NexusRoute.Api.Authentication;
using NexusRoute.Api.Hubs;
using NexusRoute.Api.Middleware;
using NexusRoute.Application.Interfaces;
using NexusRoute.Application.Services;
using NexusRoute.Application.Validators;
using NexusRoute.Domain.Services;
using NexusRoute.Infrastructure.Data;
using NexusRoute.Infrastructure.Repositories;
using NexusRoute.Infrastructure.Seed;
using NexusRoute.Simulator.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/nexusroute-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NexusRoute Mining API",
        Version = "v1",
        Description = "Real-Time Pit-to-Plant Fleet Dispatcher for Kivu Ridge Gold Operations (FICTIONAL)"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>() ?? new JwtConfiguration();
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<NexusRouteDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<ITelemetryRepository, TelemetryRepository>();
builder.Services.AddScoped<IOreMovementRepository, OreMovementRepository>();
builder.Services.AddScoped<IConvoyRepository, ConvoyRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();

builder.Services.AddScoped<ICycleTimeCalculator, CycleTimeCalculator>();
builder.Services.AddScoped<IGeofenceValidator, GeofenceValidator>();
builder.Services.AddScoped<IHealthMonitor, HealthMonitor>();
builder.Services.AddScoped<ITonnageAggregator, TonnageAggregator>();

builder.Services.AddScoped<TelemetryService>();
builder.Services.AddScoped<ConvoyTrackingService>();
builder.Services.AddScoped<ProductionService>();
builder.Services.AddScoped<AlertService>();

builder.Services.AddScoped<INotificationHub, NexusRoute.Api.Services.NotificationService>();

builder.Services.AddScoped<IValidator<NexusRoute.Application.DTOs.TelemetryDto>, TelemetryValidator>();
builder.Services.AddScoped<IValidator<NexusRoute.Application.DTOs.OreMovementDto>, OreMovementValidator>();

// DataSeeder removed - using NamoyaDataSeeder directly

builder.Services.Configure<NexusRoute.Simulator.Services.SimulatorConfiguration>(
    builder.Configuration.GetSection("Simulator"));

var enableSimulator = builder.Configuration.GetValue<bool>("Simulator:Enabled");
if (enableSimulator)
{
    builder.Services.AddHostedService<TelemetrySimulatorService>();
    builder.Services.AddHostedService<CycleRecalculationService>();
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHealthChecks()
    .AddCheck("database", () => HealthCheckResult.Healthy());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NexusRouteDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully");

        var seeder = new NamoyaDataSeeder(context, scope.ServiceProvider.GetRequiredService<ILogger<NamoyaDataSeeder>>());
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database");
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexusRoute Mining API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<DispatchHub>("/hubs/dispatch");
app.MapHealthChecks("/health");

app.MapFallbackToFile("index.html");

Log.Information("NexusRoute Mining API starting up...");
Log.Information("DISCLAIMER: Kivu Ridge Gold Operations is entirely FICTIONAL");

app.Run();
