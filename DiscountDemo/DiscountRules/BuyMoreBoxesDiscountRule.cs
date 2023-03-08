namespace DiscountDemo.DiscountRules;

/// <summary>
/// 任兩箱 88 折
/// </summary>
public class BuyMoreBoxesDiscountRule : RuleBase
{
    public readonly int BoxCount = 0;
    public readonly int PercentOff = 0;
    
    public BuyMoreBoxesDiscountRule(int boxes, int percentOff)
    {
        BoxCount = boxes;
        PercentOff = percentOff;

        this.Name = $"任 {this.BoxCount} 箱結帳 {100 - percentOff} 折";
        this.Note = "熱銷飲料，限時優惠";
    }
    
    public override IEnumerable<Discount> Process(Product[] products)
    {
        List<Product> matchedProducts = new ();
        foreach (var product in products)
        {
            matchedProducts.Add(product);
            
            if (matchedProducts.Count == this.BoxCount)
            {
                yield return new Discount
                {
                    Amount = matchedProducts.Select(p => p.Price).Sum() * PercentOff / 100,
                    Products = matchedProducts.ToArray(),
                    RuleName = this.Name
                };
                
                matchedProducts.Clear();
            }
        }
    }
}