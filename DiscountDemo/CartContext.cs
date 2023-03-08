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
}