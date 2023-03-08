namespace DiscountDemo;

public class CartContext
{
    public readonly List<Product> PurchasedItems = new ();
    public readonly List<Discount> AppliedDiscounts = new ();
    public decimal TotalPrice = 0;
}