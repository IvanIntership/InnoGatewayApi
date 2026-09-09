using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()   
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition");
    });
});

var environment = builder.Environment.EnvironmentName;
var ocelotConfigFile = environment == "Docker" ? "ocelot.Docker.json" : "ocelot.json";

builder.Configuration.AddJsonFile(ocelotConfigFile, optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowFrontend");

await app.UseOcelot();

app.Run("http://*:5000");