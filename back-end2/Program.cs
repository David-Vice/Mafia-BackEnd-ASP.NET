global using back_end.Data;
global using Microsoft.EntityFrameworkCore;
global using back_end.Models;
global using back_end.Services;
global using back_end.Services.Abstract;
global using back_end.Security;
global using back_end.Services.Concrete;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
    //for local
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    //for remote
    options.UseMySql(
                builder.Configuration.GetConnectionString("MysqlConnection"),
               new MariaDbServerVersion(new Version(10, 5,13))
            );
});
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
PrepDB.PrepPopulation(app);
