using API.DataBase;
using API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*
var connectionStringPostgreSQL = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<SitioDB>(options =>
    options.UseNpgsql(connectionStringPostgreSQL)
);
*/
var connectionStringSQLServer = builder.Configuration.GetConnectionString("SQLServerConnection");
builder.Services.AddDbContext<SitioDB>(options =>
    options.UseSqlServer(connectionStringSQLServer)
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
        /*builder =>
        {
            builder.WithOrigins(
                "http://192.168.100.44:8081", "https://192.168.56.1", "http://192.168.56.1", "http://167.250.54.76", "https://167.250.54.76",
                "http://localhost:8080", "http://localhost:8081")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });*/
});
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
app.UseCors("AllowOrigin");
app.Run();
