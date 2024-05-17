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
    public class SubCategoryService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SubCategoryService> _logger;

        public SubCategoryService(IConfiguration configuration, HttpClient httpClient, ILogger<SubCategoryService> logger)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<SubCategoryModel>> GetSubCategoryAsync()
        {
            try
            {
                var url = _configuration.GetValue<string>("AzureFunctions:GetSubCategory");
                var result = await _httpClient.GetFromJsonAsync<List<SubCategoryModel>>(url);
                return result ?? new List<SubCategoryModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subcategories.");
                return new List<SubCategoryModel>();
            }
        }

        public async Task<List<SubCategoryModel>> SortSubCategoryAsync(List<string> unsorted)
        {
            try
            {
                var allSubCategories = await GetSubCategoryAsync();
                var sortedSubCategories = new List<SubCategoryModel>();

                foreach (var subcategory in allSubCategories)
                {
                    foreach (var unsortedItem in unsorted)
                    {
                        if (subcategory.SubCategoryName.Equals(unsortedItem, StringComparison.OrdinalIgnoreCase))
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
                _logger.LogError(ex, "Error sorting subcategories.");
                return new List<SubCategoryModel>(); // Return an empty list in case of an error
            }
        }
    }
}
