using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Manero_Customer.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Manero_Customer.Services
{
    public class ProductDetailsService(HttpClient httpClient, IConfiguration configuration, ILogger<ProductDetailsService> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<ProductDetailsService> _logger = logger;

        public async Task<List<ProductDetailsModel>> MatchProductsByTitleAsync(string title)
        {
            try
            {
                
                var baseUrl = "https://maneroproductsfunction.azurewebsites.net/api/SortProduct";
                var url = $"{baseUrl}?title={Uri.EscapeDataString(title)}&code=DDouJB2A89tIcTmyQLA60nUafk_PqQDmkWjWA8d_ZAH0AzFueERmlQ%3D%3D";

                _logger.LogInformation("Requesting URL: {Url}", url);
                var result = await _httpClient.GetFromJsonAsync<List<ProductDetailsModel>>(url);
                return result ?? [];
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogError(httpEx, "Unauthorized request. Please check your API key or token.");
                return [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error matching products by title.");
                return [];
            }
        }
    }
}
