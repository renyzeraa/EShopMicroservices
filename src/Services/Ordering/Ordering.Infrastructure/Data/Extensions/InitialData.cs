namespace Ordering.Infrastructure.Data.Extensions;

internal static class InitialData
{
    private static readonly CustomerId Joao = CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa"));
    private static readonly CustomerId Maria = CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d"));

    private static readonly ProductId IPhoneX = ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"));
    private static readonly ProductId SamsungS10 = ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"));
    private static readonly ProductId HuaweiPlus = ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"));
    private static readonly ProductId XiaomiMi = ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"));

    public static IEnumerable<Customer> Customers =>
    [
        Customer.Create(Joao, "João Silva", "joao.silva@email.com"),
        Customer.Create(Maria, "Maria Souza", "maria.souza@email.com")
    ];

    public static IEnumerable<Product> Products =>
    [
        Product.Create(IPhoneX, "IPhone X", 950.00M),
        Product.Create(SamsungS10, "Samsung 10", 840.00M),
        Product.Create(HuaweiPlus, "Huawei Plus", 650.00M),
        Product.Create(XiaomiMi, "Xiaomi Mi", 450.00M)
    ];

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var joaoAddress = Address.Of("João", "Silva", "joao.silva@email.com", "Rua das Flores, 100", "Brasil", "PR", "87000");
            var mariaAddress = Address.Of("Maria", "Souza", "maria.souza@email.com", "Av. Brasil, 2000", "Brasil", "SP", "01000");

            var joaoPayment = Payment.Of("João Silva", "5555555555554444", "12/28", "355", 1);
            var mariaPayment = Payment.Of("Maria Souza", "8885555555554444", "06/30", "222", 2);

            var order1 = Order.Create(
                OrderId.Of(new Guid("9f2d1c2e-3b4a-4c5d-8e6f-7a8b9c0d1e2f")),
                Joao,
                OrderName.Of("ORD_1"),
                shippingAddress: joaoAddress,
                billingAddress: joaoAddress,
                joaoPayment);
            order1.Add(IPhoneX, 2, 950.00M);
            order1.Add(SamsungS10, 1, 840.00M);

            var order2 = Order.Create(
                OrderId.Of(new Guid("1a2b3c4d-5e6f-4a7b-8c9d-0e1f2a3b4c5d")),
                Maria,
                OrderName.Of("ORD_2"),
                shippingAddress: mariaAddress,
                billingAddress: mariaAddress,
                mariaPayment);
            order2.Add(HuaweiPlus, 1, 650.00M);
            order2.Add(XiaomiMi, 2, 450.00M);

            return [order1, order2];
        }
    }
}
