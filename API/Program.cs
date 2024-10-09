using System.Text.Json.Serialization;
using API.Extentions;
using API.Middlewares;
using Domain.SeedWork.Notification;
using HealthChecks.UI.Client;
using Infra.IoC;
using Infra.Utils.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<CrossCuttingMapper>(builder.Configuration.GetSection("CrossCuttingMapper"));
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("0.0.1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Auth API",
            Version = "0.0.1",
            Description = "API responsavel pela autenticação do dominio Air Finder",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact { Name = "Air Finder" }
        });
});


#region Local Injections
builder.Services.AddLocalServices(builder.Configuration);
builder.Services.AddLocalHttpClients(builder.Configuration);
builder.Services.AddLocalUnitOfWork(builder.Configuration);
builder.Services.AddLocalHealthChecks(builder.Configuration);
builder.Services.AddLocalSecurity(builder.Configuration);
#endregion

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis"));

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
);

builder.Services.AddHttpContextAccessor();
builder.Services.AddOptions();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("https://bvsilva.com", "https://*.bvsilva.com")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
ServiceLocator.Initialize(app.Services.GetRequiredService<IContainer>());
app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapControllers();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/0.0.1/swagger.json", "Auth API"); });
    app.ApplyMigrations();
}

app.UseMiddleware<RedisCacheMiddleware>();
app.UseMiddleware<ControllerMiddleware>();

try
{
    Log.Information("[AFAuth] Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "[AFAuth] Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

