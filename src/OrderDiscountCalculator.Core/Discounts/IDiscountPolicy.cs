namespace OrderDiscountCalculator.Core.Discounts;

public interface IDiscountPolicy
{
    string CustomerType { get; }
    decimal DiscountRate { get; }
}
