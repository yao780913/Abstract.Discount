參考以下網址

[安德魯的部落格 - 架構面試題 #4 - 抽象化設計；折扣規則的設計機制](https://columns.chicken-house.net/2020/03/10/interview-abstraction/#%E5%95%8F%E9%A1%8C-%E6%8A%98%E6%89%A3%E6%A9%9F%E5%88%B6%E5%88%B0%E5%BA%95%E6%9C%89%E5%A4%9A%E9%9B%A3%E6%90%9E)

[Github Source code](https://github.com/andrew0928/Andrew.DiscountDemo)

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

# 步驟 3, 實作第一個折扣規則

> 新增 `BuyMoreBoxesDiscountRule` 並繼承 `RuleBase`

* 定義兩個參數 `BoxCount` , `PercentOff`，決定他的箱數與折扣
* 實作方法 `Process()`

# 步驟 4, 重構

### 目標

1. 原本 `RuleBase.Process()` 定義的簽章，只接受 `Product[]` 的參數，對於折扣規則來說，資訊不太足夠
2. 顯示的資訊稍嫌混亂，想統一在一個地方輸入 `Console.WriteLine()`

### 做法

1. **新增 `CartContext`** 購物車
    * 購買的商品
    * 套用到的折扣 (由 POS 計算)
2. 新增 `POS`
    * 由店家控制該店家有哪些折扣規則
    * 計算折扣

```csharp
public class CartContext
{
    public readonly List<Discount> AppliedDiscounts = new ();
    public readonly List<Product> PurchasedItems = new ();
    public decimal TotalPrice = 0;
}

public class POS
{
    public readonly List<RuleBase> ActiveRules = new ();

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
```

> 將職責切開，由 POS 載入所有的折扣規則，並計算後將套用到的折扣規則放回購物車內  
> 這樣購物車僅需紀錄兩件事情, [1] 我購買的所有商品， [2] 套用到的折扣規則


# 步驟 5, 擴充第二個規則
* 折價券滿 1000 抵用 100，每次交易限用一次

# 大亂鬥 - 挑戰更多折扣規則
1. - [x] 指定商品 (衛生紙) 一次買 6 捲便宜 100 元
2. 指定商品 (蘋果) 單粒 23 元， 4 粒 88 元
3. - [x] 指定商品 (雞湯塊) 單盒特價 145 元，第二件 5 折
4. - [x] 指定商品 同商品 加 10 元多 1 件 (轉換: 同商品第二件 10 元)
5. - [x] 餐餐超值配, 指定鮮食 + 指定飲料 特價 ( 39元, 49元, 59元 )
6. - [x] 熱銷飲品, 限時優惠! 任 2 箱結帳 88 折!

# 進階挑戰 - 配對折扣 & 折扣排除

## 配對折扣

* 39飲料 + 39鮮食 = 39 元
* 49飲料 + 49鮮食 = 49 元
* 49飲料 + 59鮮食 = 49 元
* 59飲料 + 49鮮食 = 59 元
* 59飲料 + 59鮮食 = 59 元

## 折扣排除

> 做抽象化的目的，就是讓各個折扣規則各自獨立，不被影響

但目前這需求，擺明跟初衷不同，接著要如何解決呢？
1. **由前面的折扣規則來決定**  
   由前面的折扣規則來決定, 後面的規則是否會被排除。**前面規則的開發人員**，要撰寫是否需要跳過後面折扣的邏輯。
2. **由後面的折扣規則決定**  
   每次套用規則時，都需要檢查前面的規則。**由後面規則的開發人員撰寫**    
   那系統要提供一套查詢已套用規則的方法
3. **直接由系統決定**  
   修改 `RuleBase`，將其實作在 `POS` 內，讓全部的 Rule 遵循該規則。

> [1], [2] 方案，都需要制定開發的規定， [3] 的優點是只要根據建立好的抽象類別並實作，就可以將統一套入規則。


# Summary

* 將所有的折扣規則都放入 `POS`
* 在 `POS` 內, 對各個 rule 執行 `process`就好，不需要知道各個折扣的規則
* `CartContext` 不需要知道所有的折扣規則，僅需要紀錄購買商品與最終有套用到的折扣規則

### `RuleBase`
所有實作的折扣都要繼承 `RuleBase`

### `CartContext`
1. 所有購買的商品
2. 套用的折扣規則 (透過 `POS.Process` 計算)

### `POS`
1. 載入所有的折扣規則
2. 計算符合折扣條件的 product
3. 計算折扣的金額 
4. 計算最終結帳金額
