namespace DiscountDemo.DiscountRules;

internal class DiscountRule3 : RuleBase
{
    public string TargetTag { get; }
    public int TargetCount { get; }
    public int PercentOff { get; }

    public DiscountRule3 (string targetTag, int targetCount, int percentOff)
    {
        TargetTag = targetTag;
        TargetCount = targetCount;
        PercentOff = percentOff;

        this.Name = "滿件折扣3";
        this.Note = $"指定商品 ({targetTag})，第二件 {10 - percentOff / 10} 折";
    }

    public override IEnumerable<Discount> Process (CartContext cart)
    {
        var matched = new List<Product>();
        foreach (var p in cart.GetVisiblePurchasedItems(ExclusiveTag).Where(pi => pi.Tags.Contains(TargetTag)))
        {
            matched.Add(p);

            if (matched.Count == TargetCount)
            {
                yield return new Discount
                {
                    Amount = p.Price * this.PercentOff / 100,
                    Products = matched.ToArray(),
                    Rule = this
                };
                matched.Clear();
            }
        }
    }
}