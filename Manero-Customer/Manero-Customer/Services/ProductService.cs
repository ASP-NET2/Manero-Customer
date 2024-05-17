using Manero_Customer.Data.Models;

namespace Manero_Customer.Services
{
    public class ProductService(HttpClient httpClient, IConfiguration configuration)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;

        public async Task <List<ProductCategoryModel>> GetProducts()
        {
            try
            {
                var url = _configuration.GetValue<string>("AzureFunctions:GetAllProducts");
                var result = await _httpClient.GetFromJsonAsync<List<ProductCategoryModel>>(url);
                return result ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<List<ProductCategoryModel>> FilterProduct(string filter)
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

    }
}

