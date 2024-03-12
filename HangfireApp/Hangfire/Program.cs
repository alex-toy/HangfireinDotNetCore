using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHangfire((serviceProvider, config) =>
{

    string ConnectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    config.UseSqlServerStorage(ConnectionString);
});
builder.Services.AddHangfireServer();






var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();
