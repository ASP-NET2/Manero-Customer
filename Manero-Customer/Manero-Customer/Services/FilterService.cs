using Manero_Customer.Data.Models;
using Manero_Customer.Services;


namespace Manero_Customer.Services
{
    public class FilterService
    {
        public List<string> FilterSubCategory(List<ProductCategoryModel> models)
        {
            List<string> subcategories = new();

            foreach (var model in models) 
            {
                if (!subcategories.Contains(model.SubCategory!))
                {
                    subcategories.Add(model.SubCategory!);
                }
            }
            return subcategories;
        } 
        
        public List<string> FilterCategory(List<ProductCategoryModel> models)
        {
            List<string> categories = new();

            foreach (var model in models) 
            {
                if (model.Category != null && model.Category != "")
                {
                    if (!categories.Contains(model.Category!))
                    {
                        categories.Add(model.Category!);
                    }
                }
            }
            return categories;
        }

        
    }
}
