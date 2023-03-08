namespace DiscountDemo.DiscountRules;

public abstract class RuleBase
{
    public int Id { get; set; }
    public string Name { get; protected init; }
    public string Note { get; protected init; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 如果值不等於 null, 表示此折扣為 獨佔折扣, 
    /// 若多個折扣規則有相同的 ExclusiveTag, 則這些折扣只能套用一個。 
    /// </remarks>
    public string? ExclusiveTag = null;

    public abstract IEnumerable<Discount> Process (CartContext cart);
}