using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Solvro_Backend.Data;
using Solvro_Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Rekrutacja KN Solvro",
    });
});

string connectionString =
    $"Server={Environment.GetEnvironmentVariable("MYSQL_HOST")};" +
    $"Port={Environment.GetEnvironmentVariable("MYSQL_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("MYSQL_DATABASE")};" +
    $"Uid={Environment.GetEnvironmentVariable("MYSQL_USER")};" +
    $"Pwd={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};";

if (true) // debug purposes
{
    connectionString =
        "Server=localhost;" +
        "Port=3306;" +
        "Database=solvro;" +
        "Uid=root;" +
        "Pwd=root;";
}

builder.Services.AddDbContext<DataContext>(x => x.UseMySQL(connectionString));


// SINGLETONS
builder.Services
    .AddScoped<Database>()
    .AddScoped<IProjectRepository, ProjectRepository>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IProjectMemberMappingRepository, ProjectMemberMappingRepository>()
    .AddScoped<ITaskRepository, TaskRepository>()
    ;

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
}

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
