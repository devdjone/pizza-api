using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
}

builder.Configuration.AddEnvironmentVariables();

//https://dontpaniclabs.com/blog/post/2023/03/02/how-to-set-up-user-secrets-for-net-core-projects-in-visual-studio/#:~:text=To%20generate%20your%20user%20secrets,use%20should%20be%20stored%20here.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
