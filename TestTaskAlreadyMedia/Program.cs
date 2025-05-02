using Hangfire;
using TestTaskAlreadyMedia.Extensions;
using TestTaskAlreadyMedia.Core;
using TestTaskAlreadyMedia.Infrasructure;
using TestTaskAlreadyMedia.Core.Jobs;
using TestTaskAlreadyMedia.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureOptions(builder.Configuration);
builder.Services.AddOptions();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext();
builder.Services.AddOpenApi();
builder.Services.AddHangfire();
builder.Services.AddCoreServices();
builder.Services.AddRefitServices();
builder.Services.AddHostedService<JobsHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAutoWrapper();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();
await app.Services.ApplyMigrations();

app.Run();
