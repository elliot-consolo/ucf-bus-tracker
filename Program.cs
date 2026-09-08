using BusData.Service;
using BusData.Repositories;
using System.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IDbConnection>((sp) => 
    new NpgsqlConnection(connectionString));

builder.Services.AddHttpClient();

builder.Services.AddHostedService<ShuttlePollerService>();

builder.Services.AddControllers();
builder.Services.AddScoped<ShuttleRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UCF Bus API");
    });
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();