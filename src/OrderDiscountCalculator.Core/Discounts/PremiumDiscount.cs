namespace OrderDiscountCalculator.Core.Discounts;

public class PremiumDiscount : IDiscountPolicy
{
    public string CustomerType => "Premium";
    public decimal DiscountRate => 0.10m;
}
