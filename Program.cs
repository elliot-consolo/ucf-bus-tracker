using BusData.Service;
using BusData.Repositories;
using System.Data;
using Npgsql;
using Dapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>((sp) => 
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

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

using (var scope = app.Services.CreateScope())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString))
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            
            
            var assembly = Assembly.GetExecutingAssembly();
            
            var resourceName = $"{assembly.GetName().Name}.seed.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                string sqlScript = reader.ReadToEnd();

                connection.Execute(sqlScript);
                Console.WriteLine("Database seeded successfully from seed.sql.");
            }
            else
            {
                Console.WriteLine($"Seeding warning: Could not find embedded resource '{resourceName}'.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seeding failed: {ex.Message}");
        }
    }
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();