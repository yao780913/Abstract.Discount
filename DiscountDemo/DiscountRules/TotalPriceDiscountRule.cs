namespace DiscountDemo.DiscountRules;

public class TotalPriceDiscountRule : RuleBase
{
    public TotalPriceDiscountRule (decimal minPrice, decimal discountAmount)
    {
        Name = $"折價券滿 {minPrice} 抵用 {discountAmount}";
        Note = "每次交易限用一次";

        MinPrice = minPrice;
        DiscountAmount = discountAmount;
    }

    public decimal MinPrice { get; }
    public decimal DiscountAmount { get; }

    public override IEnumerable<Discount> Process (CartContext cart)
    {
        if (cart.TotalPrice > MinPrice)
            yield return new Discount
            {
                Amount = DiscountAmount,
                Rule = this,
                Products = cart.PurchasedItems.ToArray()
            };
    }
}