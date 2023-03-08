using DiscountDemo.DiscountRules;

namespace DiscountDemo;

public class POS
{
    public readonly List<RuleBase> ActiveRules = new ();

    public POS (IEnumerable<RuleBase> rules)
    {
        ActiveRules.AddRange(rules);
    }

    public void CheckProcess (CartContext cart)
    {
        cart.AppliedDiscounts.Clear();

        cart.TotalPrice = cart.PurchasedItems.Select(p => p.Price).Sum();
        foreach (var rule in ActiveRules)
        {
            var discounts = rule.Process(cart);
            cart.AppliedDiscounts.AddRange(discounts);

            if (rule.ExclusiveTag != null)
            {
                foreach (var d in discounts)
                {
                    foreach (var p in d.Products)
                    {
                        p.Tags.Add(rule.ExclusiveTag);
                    }
                }
            }
            
            cart.TotalPrice -= discounts.Select(d => d.Amount).Sum();
        }
    }
}