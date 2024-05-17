using Manero_Customer.Data.Models;

namespace Manero_Customer.Services
{
    public class CategoryService(IConfiguration configuration, HttpClient httpClient)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<CategoryModel>> GetCategoryAsync()
        {
            try
            {
                var url = _configuration.GetValue<string>("AzureFunctions:GetCategory");
                var result = await _httpClient.GetFromJsonAsync<List<CategoryModel>>(url);
                return result ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<List<CategoryModel>> SortCategoryAsync(List<string> Unsorted)
        {
            try
            {
                var allCategories = await GetCategoryAsync();
                var sortedCategories = new List<CategoryModel>();

                foreach (var category in allCategories)
                {
                    foreach (var unsortedItem in Unsorted)
                    {
                        if (category.CategoryName.Equals(unsortedItem))
                        {
                            sortedCategories.Add(category);
                            break; // Break inner loop if match is found
                        }
                    }
                }

                return sortedCategories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<CategoryModel>(); // Return an empty list in case of an error
            }
        }
    }
}
