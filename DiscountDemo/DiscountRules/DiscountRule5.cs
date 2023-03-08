namespace DiscountDemo.DiscountRules;

internal class DiscountRule5 : RuleBase
{
    public IEnumerable<(string food, string drink, decimal price)> DiscountTable { get; }

    public DiscountRule5 (IEnumerable<(string food, string drink, decimal price)> discountTable, string? exclusiveTag = null)
    {
        DiscountTable = discountTable;
        
        this.Name = "配對折扣";
        this.Note = "餐餐超值配 39/49/59 優惠";
        this.ExclusiveTag = exclusiveTag;
    }
    
    public override IEnumerable<Discount> Process (CartContext cart)
    {
        var purchasedItems = new List<Product>(cart.GetVisiblePurchasedItems(ExclusiveTag));

        foreach (var (food, drink, price) in DiscountTable)
        {
            var drinks = purchasedItems.Where(pi => pi.Tags.Contains(drink)).OrderByDescending(pi => pi.Price);
            var foods = purchasedItems.Where(pi => pi.Tags.Contains(food)).OrderByDescending(pi => pi.Price);

            using var enumerator1 = drinks.GetEnumerator();
            using var enumerator2 = foods.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                var p1 = enumerator1.Current;
                var p2 = enumerator2.Current;

                purchasedItems.Remove(p1);
                purchasedItems.Remove(p2);

                yield return new Discount
                {
                    Amount = p1.Price + p2.Price - price,
                    Products = new[] { p1, p2 },
                    Rule = this
                };
            }
        }
    }
}