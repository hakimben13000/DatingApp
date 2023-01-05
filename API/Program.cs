using API.Entities;
using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTPS request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("https://localhost:4200")
);

app.UseAuthentication();  // ask do you have valid token
app.UseAuthorization();  // ok you have token , now what are you allowed to do 

app.MapControllers();

app.Run();
