namespace OrderDiscountCalculator.Core.Discounts;

public class VipDiscount : IDiscountPolicy
{
    public string CustomerType => "VIP";
    public decimal DiscountRate => 0.15m;
}
