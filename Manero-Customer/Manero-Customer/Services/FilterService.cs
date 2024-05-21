using Manero_Customer.Data.Models;
using System.Collections.Generic;

namespace Manero_Customer.Services
{
    public class FilterService
    {
        public List<string> FilterSubCategory(List<ProductModel> models)
        {
            if (models == null)
            {
                return [];
            }

            HashSet<string> subcategories = [];
            foreach (var model in models)
            {
                if (!string.IsNullOrWhiteSpace(model.SubCategoryName))
                {
                    subcategories.Add(model.SubCategoryName);
                }
            }

            return new List<string>(subcategories);
        }

        public List<string> FilterCategory(List<ProductModel> models)
        {
            if (models == null)
            {
                return [];
            }

            HashSet<string> categories = [];
            foreach (var model in models)
            {
                if (!string.IsNullOrWhiteSpace(model.CategoryName))
                {
                    categories.Add(model.CategoryName);
                }
            }

            return new List<string>(categories);
        }
    }
}
