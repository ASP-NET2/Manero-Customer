namespace Manero_Customer.Data.Models;

    public class ProductDetailsModel
    {
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int Price { get; set; }
    public int Quantity { get; set; } = 1;
    public string ImageUrl { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string LongDescription { get; set; } = null!;
    public string FormatName { get; set; } = null!;
   
   
}
