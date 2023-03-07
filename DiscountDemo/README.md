﻿參考以下網址

[安德魯的部落格 - 架構面試題 #4 - 抽象化設計；折扣規則的設計機制](https://columns.chicken-house.net/2020/03/10/interview-abstraction/#%E5%95%8F%E9%A1%8C-%E6%8A%98%E6%89%A3%E6%A9%9F%E5%88%B6%E5%88%B0%E5%BA%95%E6%9C%89%E5%A4%9A%E9%9B%A3%E6%90%9E)

# 步驟 2, 定義折扣規則的抽象化介面 `RuleBase`

> **希望不影響到結帳程序 `CheckoutProcess()`**
> 在計算過程中能處理掉各種**折扣規則**

1. 找出重點: 抽象化所有折扣規則，用同樣的**規則**定義所有的行為
2. 隱藏細節:

因此，每個折扣活動都應該包含以下功能

1. 根據購物車內的商品，決定結帳時能享有那些折扣
2. 每個折扣有各自的計算方法
3. 商品有可能同時符合多項折扣活動，要按照順序計算

```csharp
public abstract class RuleBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public abstract IEnumerable<Discount> Process (Product[] products);
}

public class Discount
{
    public int Id { get; set; }
    public string RuleName { get; set; }
    public Product[] Products { get; set; }
    public decimal Amount { get; set; }
}
```