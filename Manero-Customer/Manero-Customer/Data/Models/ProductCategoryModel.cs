using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Manero_Customer.Data.Models
{
    public class ProductCategoryModel
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; } 
        public string? Author { get; set; }
        public int? Price { get; set; }
        public string? DiscountPrice { get; set; }
        public string? Image {  get; set; }
    }
}
