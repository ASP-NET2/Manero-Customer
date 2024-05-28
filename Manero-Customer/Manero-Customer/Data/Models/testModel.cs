namespace Manero_Customer.Data.Models;

public class TestModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Format { get; set; } = null!;
    public int Price { get; set; }
    public int DiscountPrice { get; set; }
    public int Quantity { get; set; }
    public string ImgUrl { get; set; } = null!;
}
