using Microsoft.EntityFrameworkCore;
using Serilog;
using StackOverflowTags.Data;
using StackOverflowTags.Endpoints;
using StackOverflowTags.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context , configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext() // Optional: Enriches log events with additional information
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt" , rollingInterval: RollingInterval.Day));

builder.Services.AddStackOverflowClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IStackOverflowService , StackOverflowService>();
builder.Services.AddHostedService<TagFetchingBackgroundService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json" , "StackOverflowTags API V1");
    c.RoutePrefix = string.Empty;
});


if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

StackOverflowTagsEndpoints.ConfigureStackOverflowTagsEndpoints(app);

app.Run();

public partial class Program { }//needed for tests