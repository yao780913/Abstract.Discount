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

        foreach (var discounts in ActiveRules.Select(rule => rule.Process(cart)))
        {
            cart.AppliedDiscounts.AddRange(discounts);
            cart.TotalPrice -= discounts.Select(d => d.Amount).Sum();
        }
    }
}