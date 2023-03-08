namespace DiscountDemo;

public class CartContext
{
    public readonly List<Discount> AppliedDiscounts = new ();
    public readonly List<Product> PurchasedItems = new ();
    public decimal TotalPrice = 0;
}