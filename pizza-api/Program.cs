using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using pizza_api.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<pizza_apiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("pizza_apiContext") ?? throw new InvalidOperationException("Connection string 'pizza_apiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
//builder.Configuration.AddEnvironmentVariables();


//builder.WebHost.UseUrls("http://*:8080");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
}

builder.Configuration.AddEnvironmentVariables();

//https://devfile.io/
//https://dontpaniclabs.com/blog/post/2023/03/02/how-to-set-up-user-secrets-for-net-core-projects-in-visual-studio/#:~:text=To%20generate%20your%20user%20secrets,use%20should%20be%20stored%20here.

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
