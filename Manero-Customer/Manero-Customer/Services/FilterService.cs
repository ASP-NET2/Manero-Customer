using Manero_Customer.Data.Models;
using System.Collections.Generic;

namespace Manero_Customer.Services
{
    public class FilterService
    {
        public List<string> FilterSubCategory(List<ProductCategoryModel> models)
        {
            if (models == null)
            {
                return new List<string>();
            }

            HashSet<string> subcategories = new();
            foreach (var model in models)
            {
                if (!string.IsNullOrWhiteSpace(model.SubCategory))
                {
                    subcategories.Add(model.SubCategory);
                }
            }

            return new List<string>(subcategories);
        }

        public List<string> FilterCategory(List<ProductCategoryModel> models)
        {
            if (models == null)
            {
                return new List<string>();
            }

            HashSet<string> categories = new();
            foreach (var model in models)
            {
                if (!string.IsNullOrWhiteSpace(model.Category))
                {
                    categories.Add(model.Category);
                }
            }

            return new List<string>(categories);
        }
    }
}
