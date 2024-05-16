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
                var url = _configuration.GetValue<string>("ProductAPIs:GetAllProducts");
                var result = await _httpClient.GetFromJsonAsync<List<ProductCategoryModel>>(url);
                return result ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }
    }
}
