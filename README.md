# Order Discount Calculator

A small C# console application for the Staff Engineer technical screening exercise.

## Requirements

- .NET 10 SDK
- No database or external service required
- Internet access on the first test run to restore xUnit test packages

## Run

From the solution folder:

```bash
dotnet run --project src/OrderDiscountCalculator
```

Run the unit tests:

```bash
dotnet test OrderDiscountCalculator.sln
```

The solution can also be opened in Visual Studio 2022 with the .NET 8 SDK installed.

## Sample output

```text
ORDER DISCOUNT CALCULATOR
-------------------------------------------------------------------------------------
Order ID   Customer     Type             Amount     Discount   Final Amount
-------------------------------------------------------------------------------------
101        Amit         Regular         1000.00        50.00         950.00
102        Priya        Premium         2000.00       200.00        1800.00
103        Rahul        VIP             3000.00       450.00        2550.00
```

## Design

- `Order` holds the order details.
- `IDiscountPolicy` defines the customer type and discount rate.
- `RegularDiscount`, `PremiumDiscount`, and `VipDiscount` provide the rates.
- `OrderService` validates an order and calculates its discount and final amount.
- `Program.cs` sets up the policies and prints three sample orders.

The calculation service does not have `if/else` or `switch` statements for specific customer types. To add a discount, create another class implementing `IDiscountPolicy` and register it when constructing `OrderService`. This is a simple use of the Strategy pattern and the Open/Closed Principle.

## Assumptions

- An order has one customer type and one applicable discount.
- Discounts are applied to the full order amount; tax, coupons, and shipping are out of scope.
- An amount of zero is valid. Negative amounts and missing customer information are rejected.
- Unknown customer types are rejected instead of silently receiving a different discount.
- Money uses `decimal`. Discounts are rounded to two decimal places, halfway away from zero.
- Customer type matching is case-insensitive.
- The console examples are hardcoded for demonstration; no UI or database is required.

## Tests

The xUnit tests cover all three discounts, case-insensitive matching, zero and negative amounts, missing customer data, an unknown customer type, rounding, and adding a new discount policy without modifying the service.
