using Hangfire;
using TestTaskAlreadyMedia.Extensions;
using TestTaskAlreadyMedia.Core;
using TestTaskAlreadyMedia.Infrasructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureOptions(builder.Configuration);
builder.Services.AddOptions();
builder.Services.AddControllers();
builder.Services.AddDbContext();
builder.Services.AddOpenApi();
builder.Services.AddHangfire();
builder.Services.AddRefitServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
await app.Services.ApplyMigrations();

app.Run();
