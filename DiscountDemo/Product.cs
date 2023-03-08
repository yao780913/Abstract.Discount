using System.Runtime.CompilerServices;

namespace DiscountDemo;

public class Product
{
    public int Id { get; set; }
    public string SKU { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public HashSet<string> Tags { get; set; }

    public string TagsValue
    {
        get
        {
            if (this.Tags == null || this.Tags.Count == 0)
                return string.Empty;

            return string.Join(",", this.Tags.Select(t => "#" + t));
        }
    }
        
}