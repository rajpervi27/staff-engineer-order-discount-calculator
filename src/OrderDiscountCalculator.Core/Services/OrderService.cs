using OrderDiscountCalculator.Core.Discounts;
using OrderDiscountCalculator.Core.Models;

namespace OrderDiscountCalculator.Core.Services;

public class OrderService
{
    private readonly Dictionary<string, IDiscountPolicy> _discounts;

    public OrderService(IEnumerable<IDiscountPolicy> discountPolicies)
    {
        ArgumentNullException.ThrowIfNull(discountPolicies);

        _discounts = new Dictionary<string, IDiscountPolicy>(StringComparer.OrdinalIgnoreCase);

        foreach (var policy in discountPolicies)
        {
            if (policy is null || string.IsNullOrWhiteSpace(policy.CustomerType))
                throw new ArgumentException("A discount policy must have a customer type.", nameof(discountPolicies));

            if (policy.DiscountRate < 0 || policy.DiscountRate > 1)
                throw new ArgumentException("Discount rate must be between 0 and 1.", nameof(discountPolicies));

            if (!_discounts.TryAdd(policy.CustomerType, policy))
                throw new ArgumentException($"Duplicate discount type: {policy.CustomerType}", nameof(discountPolicies));
        }
    }

    public OrderResult Calculate(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.OrderId <= 0)
            throw new ArgumentException("Order ID must be greater than zero.", nameof(order));

        if (string.IsNullOrWhiteSpace(order.CustomerName))
            throw new ArgumentException("Customer name is required.", nameof(order));

        if (order.Amount < 0)
            throw new ArgumentException("Order amount cannot be negative.", nameof(order));

        if (string.IsNullOrWhiteSpace(order.CustomerType) ||
            !_discounts.TryGetValue(order.CustomerType, out var policy))
            throw new ArgumentException($"Unsupported customer type: {order.CustomerType}", nameof(order));

        decimal discountAmount = Math.Round(order.Amount * policy.DiscountRate, 2, MidpointRounding.AwayFromZero);

        return new OrderResult
        {
            OrderId = order.OrderId,
            CustomerName = order.CustomerName.Trim(),
            CustomerType = policy.CustomerType,
            OrderAmount = order.Amount,
            DiscountAmount = discountAmount,
            FinalAmount = order.Amount - discountAmount
        };
    }
}
