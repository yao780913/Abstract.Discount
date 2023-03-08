namespace DiscountDemo;

public class CartContext
{
    public readonly List<Discount> AppliedDiscounts = new ();
    private readonly List<Product> _purchasedItems = new ();
    public decimal TotalPrice = 0;

    public CartContext (IEnumerable<Product> products)
    {
        _purchasedItems.AddRange(products);
    }

    public List<Product> PurchasedItems => _purchasedItems;
    
    /// <summary>
    /// 若多個折扣規則有相同的 ExclusiveTag, 則這些折扣只能套用一個
    /// </summary>
    /// <param name="exclusiveTag"></param>
    /// <returns></returns>
    public IEnumerable<Product> GetVisiblePurchasedItems (string? exclusiveTag)
    {
        if (string.IsNullOrEmpty(exclusiveTag))
            return this.PurchasedItems;
        
        return this.PurchasedItems.Where(p => !p.Tags.Contains(exclusiveTag));
    }
}