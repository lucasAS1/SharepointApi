using SharepointApiIIntegration.Domain.Interfaces.Agents;
using SharepointApiIIntegration.Domain.Model.Settings;
using SharepointApiIntegration.Infrastructure.GraphService.Sharepoint;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Settings"));

//Add Singletons
builder.Services.AddSingleton<ISharepointAgent, SharepointAgent>();

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