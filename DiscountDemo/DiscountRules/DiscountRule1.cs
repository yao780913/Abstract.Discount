﻿namespace DiscountDemo.DiscountRules;

internal class DiscountRule1 : RuleBase
{
    public string TargetTag { get; }
    public int MinCount { get; }
    public decimal DiscountAmount { get; } = 0;

    public DiscountRule1 (string targetTag, int minBuyCount, decimal discountAmount)
    {
        TargetTag = targetTag;
        MinCount = minBuyCount;
        DiscountAmount = discountAmount;

        this.Name = "滿件折扣1";
        this.Note = $"指定商品 ({targetTag}) 一次買 {minBuyCount} 捲便宜 {discountAmount} 元";
    }

    public override IEnumerable<Discount> Process (CartContext cart)
    {
        var matchedProducts = new List<Product>();
        foreach (var p in cart.PurchasedItems.Where(pi => pi.Tags.Contains(TargetTag)))
        {
            matchedProducts.Add(p);

            if (matchedProducts.Count == MinCount)
            {
                yield return new Discount
                {
                    Amount = this.DiscountAmount,
                    Products = matchedProducts.ToArray(),
                    Rule = this
                };
                matchedProducts.Clear();
            }
        }
        
        
    }
}