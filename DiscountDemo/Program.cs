// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using DiscountDemo;
using DiscountDemo.DiscountRules;

internal class Program
{
    private static void Main (string[] args)
    {
        var products = LoadProducts();
        if (products == null) throw new ArgumentNullException(nameof(products));

        foreach (var p in products) Console.WriteLine($"- {p.Name}  {p.Price:C}");

        Console.WriteLine($"Total: {CheckoutProcess(products.ToArray(), LoadRules().ToArray()):C}");

        Console.ReadLine();
    }

    private static IEnumerable<RuleBase> LoadRules()
    {
        yield return new BuyMoreBoxesDiscountRule(2, 12);
    }


    private static decimal CheckoutProcess (Product[] products)
    {
        return products.Sum(p => p.Price);
    }

    private static decimal CheckoutProcess (Product[] products, RuleBase[] rules)
    {
        var discounts = new List<Discount>();

        foreach (var rule in rules) 
            discounts.AddRange(rule.Process(products));

        var amountWithoutDiscount = CheckoutProcess(products);
        decimal totalDiscount = 0;

        foreach (var discount in discounts)
        {
            totalDiscount += discount.Amount;
            Console.WriteLine($"- 符合折扣 [{discount.RuleName}], 折抵 {discount.Amount} 元");
        }

        return amountWithoutDiscount - totalDiscount;
    }

    private static IEnumerable<Product>? LoadProducts ()
    {
        var text = File.ReadAllText(@"products.json", Encoding.UTF8);
        return JsonSerializer.Deserialize<Product[]>(text, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}