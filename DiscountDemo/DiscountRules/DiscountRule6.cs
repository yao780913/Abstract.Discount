namespace DiscountDemo.DiscountRules;

internal class DiscountRule6 : RuleBase
{
    public string TargetTag { get; }
    public int PercentOff { get; }

    public DiscountRule6 (string targetTag, int percentOff)
    {
        TargetTag = targetTag;
        PercentOff = percentOff;

        this.Name = "滿件折扣6";
        this.Note = $"熱銷飲品, 限時優惠! 任 2 箱結帳 {100 - PercentOff} 折!";
    }

    public override IEnumerable<Discount> Process (CartContext cart)
    {
        var matched = new List<Product>();
        foreach (var p in cart.PurchasedItems.Where(pi => pi.Tags.Contains(TargetTag)).OrderByDescending(pi => pi.Price))
        {
            matched.Add(p);

            if (matched.Count == 2)
            {
                yield return new Discount
                {
                    Amount = matched.Sum(m => m.Price) * PercentOff / 100,
                    Products = matched.ToArray(),
                    Rule = this
                };
                
                matched.Clear();
            }
        }
    }
}