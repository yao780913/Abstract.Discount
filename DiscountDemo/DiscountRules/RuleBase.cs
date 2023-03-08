namespace DiscountDemo.DiscountRules;

public abstract class RuleBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public abstract IEnumerable<Discount> Process (Product[] products);

    public abstract IEnumerable<Discount> Process (CartContext products);
}