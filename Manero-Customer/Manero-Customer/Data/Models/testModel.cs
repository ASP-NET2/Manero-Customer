namespace Manero_Customer.Data.Models;

public class TestModel
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Format { get; set; } = null!;
    public int Price { get; set; }
    public int DiscountPrice { get; set; }
    public int Quantity { get; set; }
    public string ImgUrl { get; set; } = null!;
    public string UserId { get; set; } = null!;
}
