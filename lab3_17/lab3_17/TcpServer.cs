using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using lab3_17.Services;
using System.Text.Encodings.Web;

public class TcpServer
{
    private readonly WeatherService _weatherService;
    private readonly ILogger _logger;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public TcpServer(
        WeatherService weatherService,
        ILogger<TcpServer> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        _logger.LogInformation("TCP Server started on port 5000.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(cancellationToken);
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        _logger.LogInformation("Client connected.");
        using (client)
        await using (var stream = client.GetStream())
        {
            var buffer = new byte[4096];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            _logger.LogInformation($"Request: {request}");
            try
            {
                var requestData = JsonSerializer.Deserialize<Dictionary<string, string>>(request, JsonOptions);
                if (requestData == null || !requestData.TryGetValue("command", out var commandRaw))
                {
                    await Send(stream, new { success = false, message = "Invalid command." });
                    return;
                }

                var command = commandRaw.ToLower();
                switch (command)
                {
                    case "get_cities":
                    {
                        var (success, message, cities) = await _weatherService.GetCitiesAsync();
                        await Send(stream, new { success, message, data = cities });
                        break;
                    }
                    case "get_weather_history":
                    {
                        requestData.TryGetValue("city", out var cityName);
                        var (success, message, history) = await _weatherService.GetWeatherHistoryAsync(cityName ?? string.Empty);
                        await Send(stream, new { success, message, data = history });
                        break;
                    }
                    case "get_predicted_weather":
                    {
                        requestData.TryGetValue("city", out var cityName);
                        var (success, message, record) = await _weatherService.GetPredictedWeatherAsync(cityName ?? string.Empty);
                        await Send(stream, new { success, message, data = record });
                        break;
                    }
                    case "add_location":
                    {
                        requestData.TryGetValue("city", out var cityName);
                        var (success, message, city) = await _weatherService.AddLocationAsync(cityName ?? string.Empty);
                        await Send(stream, new { success, message, data = city });
                        break;
                    }
                    case "save_weather":
                    {
                        requestData.TryGetValue("city", out var cityName);
                        requestData.TryGetValue("temperature", out var temperature);
                        requestData.TryGetValue("description", out var description);
                        var (success, message, record) =
                            await _weatherService.SaveWeatherAsync(cityName ?? string.Empty, temperature ?? string.Empty,
                                description ?? string.Empty);
                        await Send(stream, new { success, message, data = record });
                        break;
                    }
                    case "update_weather":
                    {
                        requestData.TryGetValue("city", out var cityName);
                        requestData.TryGetValue("date", out var date);
                        requestData.TryGetValue("temperature", out var temperature);
                        requestData.TryGetValue("description", out var description);
                        var (success, message, record) =
                            await _weatherService.UpdateWeatherAsync(
                                cityName ?? string.Empty,
                                date ?? string.Empty,
                                temperature ?? string.Empty,
                                description ?? string.Empty);
                        await Send(stream, new { success, message, data = record });
                        break;
                    }
                    default:
                        await Send(stream, new { success = false, message = "Unsupported command." });
                        break;
                }
            }
            catch (JsonException ex)
            {
                await Send(stream, new { success = false, message = $"Invalid JSON format: {ex.Message}" });
            }
            catch (Exception ex)
            {
                await Send(stream, new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
    private async Task Send(NetworkStream stream, object obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        await stream.WriteAsync(bytes, 0, bytes.Length);
        _logger.LogInformation($"Responce: {json}");
    }
}
