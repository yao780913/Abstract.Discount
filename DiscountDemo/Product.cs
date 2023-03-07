namespace DiscountDemo;

public class Product
{
    public int Id { get; set; }
    public string SKU { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public HashSet<string> Tags { get; set; }
}