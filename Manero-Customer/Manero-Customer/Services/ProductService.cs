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
    public class ProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ProductService> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<ProductService> _logger = logger;

        public async Task<List<ProductModel>> GetProducts()
        {
            try
            {
                var url = _configuration.GetValue<string>("AzureFunctions:GetAllProducts");
                var result = await _httpClient.GetFromJsonAsync<List<ProductModel>>(url);
                return result ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products.");
                return [];
            }
        }

        public async Task<List<ProductModel>> FilterProduct(Dictionary<string, string> filters)
        {
            try
            {
                var baseUrl = "https://maneroproductsfunction.azurewebsites.net/api/SortProduct";
                var query = string.Join("&", filters.Select(filter => $"{Uri.EscapeDataString(filter.Key)}={Uri.EscapeDataString(filter.Value)}&code=DDouJB2A89tIcTmyQLA60nUafk_PqQDmkWjWA8d_ZAH0AzFueERmlQ%3D%3D"));

                var builder = new UriBuilder(baseUrl)
                {
                    Query = query
                };

                var url = builder.ToString();
                var result = await _httpClient.GetFromJsonAsync<List<ProductModel>>(url);
                return result ?? new List<ProductModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering products.");
                return new List<ProductModel>();
            }
        }

        public async Task<ProductModel?> FilterSingleProduct(Dictionary<string, string> filters)
        {
            try
            {
                var baseUrl = "https://maneroproductsfunction.azurewebsites.net/api/SortProduct";
                var query = string.Join("&", filters.Select(filter => $"{Uri.EscapeDataString(filter.Key)}={Uri.EscapeDataString(filter.Value)}&code=DDouJB2A89tIcTmyQLA60nUafk_PqQDmkWjWA8d_ZAH0AzFueERmlQ%3D%3D"));

                var builder = new UriBuilder(baseUrl)
                {
                    Query = query
                };

                var url = builder.ToString();
                var result = await _httpClient.GetFromJsonAsync<List<ProductModel>>(url);
                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering single product.");
                return null;
            }
        }

    }
}
