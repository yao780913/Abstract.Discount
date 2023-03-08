using System.Runtime.InteropServices;

namespace DiscountDemo.DiscountRules;

internal class DiscountRule4 : RuleBase
{
    public string TargetTag { get; }
    public decimal DiscountAmount { get; }

    public DiscountRule4 (string targetTag, decimal discountAmount, string exclusiveTag)
    {
        TargetTag = targetTag;
        DiscountAmount = discountAmount;
        
        this.Name = "滿件折扣4";
        this.Note = $"指定商品 同商品 加 {discountAmount} 元多 1 件";
        this.ExclusiveTag = exclusiveTag;
    }

    public override IEnumerable<Discount> Process (CartContext cart)
    {
        var matched = new List<Product>();

        foreach (var sku in cart.GetVisiblePurchasedItems(this.ExclusiveTag).Where(pi => pi.Tags.Contains(TargetTag)).Select(pi => pi.SKU).Distinct())
        {
            matched.Clear();

            foreach (var p in cart.GetVisiblePurchasedItems(this.ExclusiveTag).Where(pi => pi.SKU == sku))
            {
                matched.Add(p);
                if (matched.Count == 2)
                {
                    yield return new Discount
                    {
                        Amount = this.DiscountAmount,
                        Products = matched.ToArray(),
                        Rule = this
                    };
                    
                    matched.Clear();
                }
            }
        }
    }
}