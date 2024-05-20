using Calabonga.AspNetCore.AppDefinitions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.AddDefinitions(typeof(Program));

var app = builder.Build();

app.UseDefinitions();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();