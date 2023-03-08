using DiscountDemo.DiscountRules;

namespace DiscountDemo;

public class Discount
{
    public int Id { get; set; }
    public RuleBase Rule { get; set; }
    public Product[] Products { get; set; }
    
    /// <summary>
    /// 折扣金額
    /// </summary>
    public decimal Amount { get; set; }
}