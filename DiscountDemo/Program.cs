// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using DiscountDemo;
using DiscountDemo.DiscountRules;

internal class Program
{
    private static int _seed;

    private static void Main (string[] args)
    {
        var cart = new CartContext();
        var pos = new POS(LoadRules());

        cart.PurchasedItems.AddRange(LoadProducts());

        pos.CheckProcess(cart);

        Console.WriteLine("購買商品");
        Console.WriteLine("------------------------------------------");
        
        foreach (var product in cart.PurchasedItems)
            Console.WriteLine(
                $"- {product.Id,02}, [{product.SKU}], {product.Price,8:C}, {product.Name}, {product.TagsValue}");

        Console.WriteLine();
        Console.WriteLine("折扣:");
        Console.WriteLine("------------------------------------------");
        
        foreach (var d in cart.AppliedDiscounts)
        {
            Console.WriteLine($"- 折抵 {d.Amount,8:C}, {d.Rule.Name}, ({d.Rule.Note})");
            foreach (var p in d.Products) Console.WriteLine($"  * 符合: {p.Id,02}, [{p.SKU}], {p.Name}, {p.TagsValue}");
        }

        Console.WriteLine();
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"結帳金額    {cart.TotalPrice:C}");

        Console.ReadLine();
    }

    private static IEnumerable<RuleBase> LoadRules ()
    {
        // yield return new BuyMoreBoxesDiscountRule(2, 12);
        // yield return new TotalPriceDiscountRule(1000, 100);
        yield return new DiscountRule1("衛生紙", 6, 100);
        yield break;
    }

    private static IEnumerable<Product>? LoadProducts ()
    {
        var text = File.ReadAllText(@"products3.json", Encoding.UTF8);
        foreach (var product in JsonSerializer.Deserialize<Product[]>(text, new JsonSerializerOptions
                 {
                     PropertyNameCaseInsensitive = true
                 })!)
        {
            _seed++;
            product.Id = _seed;
            yield return product;
        }
    }
}