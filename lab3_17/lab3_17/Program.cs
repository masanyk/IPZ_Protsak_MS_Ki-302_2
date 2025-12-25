using lab3_17;
using lab3_17.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<TcpServer>();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<WeatherService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

await host.RunAsync();
