namespace Manero_Customer.Data.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int? Price { get; set; }
    }

    public class Cart
    {
        public string Id { get; set; }
        public string PartitionKey { get; set; }
        public string CartName { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Product> Products { get; set; }
    }
}
