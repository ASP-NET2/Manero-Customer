using Manero_Customer.Data.Models;
using Microsoft.Identity.Client;

namespace Manero_Customer.Services
{
    public class SubCategoryService(IConfiguration configuration, HttpClient httpClient)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<SubCategoryModel>> GetSubCategoryAsync()
        {
            try
            {
                var url = _configuration.GetValue<string>("AzureFunctions:GetSubCategory");
                var result = await _httpClient.GetFromJsonAsync<List<SubCategoryModel>>(url);
                return result ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        public async Task<List<SubCategoryModel>> SortSubCategoryAsync(List<string> Unsorted)
        {
            try
            {
                var allSubCategories = await GetSubCategoryAsync();
                var sortedSubCategories = new List<SubCategoryModel>();

                foreach (var subcategory in allSubCategories)
                {
                    foreach (var unsortedItem in Unsorted)
                    {
                        if (subcategory.SubCategoryName.Equals(unsortedItem))
                        {
                            sortedSubCategories.Add(subcategory);
                            break; // Break inner loop if match is found
                        }
                    }
                }

                return sortedSubCategories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<SubCategoryModel>(); // Return an empty list in case of an error
            }

        }
    }
}
