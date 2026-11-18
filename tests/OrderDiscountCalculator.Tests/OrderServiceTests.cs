using OrderDiscountCalculator.Core.Discounts;
using OrderDiscountCalculator.Core.Models;
using OrderDiscountCalculator.Core.Services;
using Xunit;

namespace OrderDiscountCalculator.Tests;

public class OrderServiceTests
{
    private readonly OrderService _service = new(new IDiscountPolicy[]
    {
        new RegularDiscount(),
        new PremiumDiscount(),
        new VipDiscount()
    });

    [Theory]
    [InlineData("Regular", 1000, 50, 950)]
    [InlineData("Premium", 1000, 100, 900)]
    [InlineData("VIP", 1000, 150, 850)]
    [InlineData("vip", 1000, 150, 850)]
    [InlineData("Regular", 0, 0, 0)]
    public void Calculate_ReturnsExpectedAmounts(string customerType, int amount, int expectedDiscount, int expectedFinal)
    {
        var result = _service.Calculate(new Order
        {
            OrderId = 1,
            CustomerName = "Test Customer",
            CustomerType = customerType,
            Amount = amount
        });

        Assert.Equal(expectedDiscount, result.DiscountAmount);
        Assert.Equal(expectedFinal, result.FinalAmount);
    }

    [Fact]
    public void Calculate_RoundsDiscountToTwoDecimalPlaces()
    {
        var result = _service.Calculate(new Order
        {
            OrderId = 1, CustomerName = "Test", CustomerType = "Regular", Amount = 10.10m
        });

        Assert.Equal(0.51m, result.DiscountAmount);
        Assert.Equal(9.59m, result.FinalAmount);
    }

    [Fact]
    public void Calculate_RejectsNegativeAmount()
    {
        Assert.Throws<ArgumentException>(() => _service.Calculate(NewOrder(amount: -1)));
    }

    [Fact]
    public void Calculate_RejectsUnknownCustomerType()
    {
        var order = NewOrder();
        order.CustomerType = "Unknown";

        Assert.Throws<ArgumentException>(() => _service.Calculate(order));
    }

    [Fact]
    public void Calculate_RejectsMissingName()
    {
        var order = NewOrder();
        order.CustomerName = " ";

        Assert.Throws<ArgumentException>(() => _service.Calculate(order));
    }

    [Fact]
    public void Calculate_RejectsInvalidOrderId()
    {
        var order = NewOrder();
        order.OrderId = 0;

        Assert.Throws<ArgumentException>(() => _service.Calculate(order));
    }

    [Fact]
    public void Calculate_RejectsNullOrder()
    {
        Assert.Throws<ArgumentNullException>(() => _service.Calculate(null!));
    }

    [Fact]
    public void Calculate_UsesNewDiscountWithoutChangingService()
    {
        var service = new OrderService(new IDiscountPolicy[] { new EmployeeDiscount() });
        var order = NewOrder();
        order.CustomerType = "Employee";

        var result = service.Calculate(order);

        Assert.Equal(200m, result.DiscountAmount);
        Assert.Equal(800m, result.FinalAmount);
    }

    private static Order NewOrder(decimal amount = 1000m) => new()
    {
        OrderId = 1, CustomerName = "Test", CustomerType = "Regular", Amount = amount
    };

    private class EmployeeDiscount : IDiscountPolicy
    {
        public string CustomerType => "Employee";
        public decimal DiscountRate => 0.20m;
    }
}
