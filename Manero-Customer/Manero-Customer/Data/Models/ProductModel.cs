namespace Manero_Customer.Data.Models
{
    public class ProductModel
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string DiscountPrice { get; set; } = null!;
        public string SubCategoryName { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string LongDescription { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string Language {  get; set; } = null!;
        public string Pages { get; set; } = null!;
        public string PublishDate { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string FormatName { get; set; } = null!;
        public bool OnSale { get; set; } = false;
        public bool FeaturedProduct { get; set; } = false;
        public bool BestSeller { get; set; } = false;
        public bool IsFavorite { get; set; } = false;
    }
}
