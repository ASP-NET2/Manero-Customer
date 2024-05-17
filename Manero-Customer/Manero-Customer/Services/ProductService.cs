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
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductService> _logger;

        public ProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<ProductCategoryModel>> GetProducts()
        {
            try
            {
                var url = _configuration.GetValue<string>("AzureFunctions:GetAllProducts");
                var result = await _httpClient.GetFromJsonAsync<List<ProductCategoryModel>>(url);
                return result ?? new List<ProductCategoryModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products.");
                return new List<ProductCategoryModel>();
            }
        }

        public async Task<List<ProductCategoryModel>> FilterProduct(string filter)
        {
            try
            {
                var baseUrl = "https://maneroproductsfunction.azurewebsites.net/api/SortProduct";
                var query = $"Category={Uri.EscapeDataString(filter)}&code=DDouJB2A89tIcTmyQLA60nUafk_PqQDmkWjWA8d_ZAH0AzFueERmlQ%3D%3D";

                var builder = new UriBuilder(baseUrl)
                {
                    Query = query
                };

                var url = builder.ToString();
                var result = await _httpClient.GetFromJsonAsync<List<ProductCategoryModel>>(url);
                return result ?? new List<ProductCategoryModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering products.");
                return new List<ProductCategoryModel>();
            }
        }
    }
}
