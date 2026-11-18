using OrderDiscountCalculator.Core.Discounts;
using OrderDiscountCalculator.Core.Models;
using OrderDiscountCalculator.Core.Services;

var service = new OrderService(new IDiscountPolicy[]
{
    new RegularDiscount(),
    new PremiumDiscount(),
    new VipDiscount()
});

var orders = new List<Order>
{
    new() { OrderId = 101, CustomerName = "Amit", CustomerType = "Regular", Amount = 1000m },
    new() { OrderId = 102, CustomerName = "Priya", CustomerType = "Premium", Amount = 2000m },
    new() { OrderId = 103, CustomerName = "Rahul", CustomerType = "VIP", Amount = 3000m }
};

Console.WriteLine("ORDER DISCOUNT CALCULATOR");
Console.WriteLine(new string('-', 85));
Console.WriteLine($"{"Order ID",-10} {"Customer",-12} {"Type",-10} {"Amount",12} {"Discount",12} {"Final Amount",14}");
Console.WriteLine(new string('-', 85));

foreach (var order in orders)
{
    try
    {
        var result = service.Calculate(order);
        Console.WriteLine($"{result.OrderId,-10} {result.CustomerName,-12} {result.CustomerType,-10} {result.OrderAmount,12:F2} {result.DiscountAmount,12:F2} {result.FinalAmount,14:F2}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Order {order.OrderId}: {ex.Message}");
    }
}
