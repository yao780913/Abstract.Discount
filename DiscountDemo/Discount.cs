namespace DiscountDemo;

public class Discount
{
    public int Id { get; set; }
    public string RuleName { get; set; }
    public Product[] Products { get; set; }
    public decimal Amount { get; set; }
}