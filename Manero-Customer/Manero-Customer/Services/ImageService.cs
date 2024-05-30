using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace Manero_Customer.Services;

public class ImageService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public ImageService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string> UploadImageAsync(IBrowserFile file)
    {
        try
        {
            var url = "https://blobmanero.azurewebsites.net/api/Upload";

            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream(maxAllowedSize: 104857600);
            using var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, "file", file.Name);

            var response = await _http.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error response: {errorContent}");
                response.EnsureSuccessStatusCode();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent.Trim('"');
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HttpRequestException: {httpEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }
}
