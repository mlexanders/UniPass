using Calabonga.AspNetCore.AppDefinitions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.AddDefinitions(typeof(Program));

var app = builder.Build();

app.UseDefinitions();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();