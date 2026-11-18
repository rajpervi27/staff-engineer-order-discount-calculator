namespace OrderDiscountCalculator.Core.Discounts;

public class RegularDiscount : IDiscountPolicy
{
    public string CustomerType => "Regular";
    public decimal DiscountRate => 0.05m;
}
