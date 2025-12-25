using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace lab2_17.Requests;

internal static class RequestClient
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static RequestResult<T> Send<T>(object requestModel)
    {
        try
        {
            var request = JsonSerializer.Serialize(requestModel, JsonOptions);
            var bytesToSend = Encoding.UTF8.GetBytes(request);

            using var client = new TcpClient(ServerAddress, ServerPort);
            using var stream = client.GetStream();
            stream.Write(bytesToSend, 0, bytesToSend.Length);

            var buffer = new byte[4096];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            var responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var response = JsonSerializer.Deserialize<ResponseWrapper<T>>(responseJson, JsonOptions);
            if (response != null)
            {
                return new RequestResult<T>
                {
                    Success = response.success,
                    Message = response.message ?? string.Empty,
                    Data = response.data
                };
            }

            return new RequestResult<T> { Success = false, Message = "Невідомий формат відповіді", Data = default };
        }
        catch (Exception ex)
        {
            return new RequestResult<T> { Success = false, Message = ex.Message, Data = default };
        }
    }

    private class ResponseWrapper<T>
    {
        public bool success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}

public class RequestResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}
