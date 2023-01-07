using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // AddApplicationServices is in Extensions folder that contain ApplicationServiceExtensions.cs file with AddDbContext and AddCors and AddScoped
builder.Services.AddIdentityServices(builder.Configuration); // AddIdentityServices is in Extensions folder that contain IdentityServiceExtensions.cs file with AddIdentityCore and AddSignInManager and AddJwtBearer
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTPS request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("https://localhost:4200")
); // AllowAnyHeader and AllowAnyMethod is for the CORS policy and WithOrigins is for the origin of the client app (Angular app) that we want to allow to access our API (localhost:5001) 

app.UseAuthentication();  // ask do you have valid token
app.UseAuthorization();  // ok you have token , now what are you allowed to do 

app.MapControllers();

using var scope = app.Services.CreateScope(); // create scope for dbcontext
var services = scope.ServiceProvider; // get services from scope
try
{
    var context = services.GetRequiredService<DataContext>(); // get data context
    await context.Database.MigrateAsync(); // create database if not exist and apply all migrations
    await Seed.SeedUsers(context); // seed data
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
