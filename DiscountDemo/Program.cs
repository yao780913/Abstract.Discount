// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using DiscountDemo;

var products = LoadProducts();

foreach (var p in products)
{
    Console.WriteLine($"- {p.Name}      {p.Price:C}");
}

Console.WriteLine($"Total: {CheckoutProcess(products.ToArray()):C}");

Console.ReadLine();

decimal CheckoutProcess (Product[] products)
{
    return products.Sum(p => p.Price);
}

IEnumerable<Product> LoadProducts ()
{
    var text = File.ReadAllText(@"products.json", Encoding.UTF8);
    return JsonSerializer.Deserialize<Product[]>(text, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });
}