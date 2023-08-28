using Microsoft.Extensions.Options;
using RemotelyControlledRobot.WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseHttpsRedirection();
app.MapControllers();

app.MapHub<RobotHub>("/robothub");


app.Run();