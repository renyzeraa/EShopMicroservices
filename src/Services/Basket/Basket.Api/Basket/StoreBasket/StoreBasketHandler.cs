namespace Basket.Api.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
  public StoreBasketCommandValidator()
  {
    RuleFor(x => x.Cart).NotNull().WithMessage("Cart is required.");
    RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required.");
  }
}

internal class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
  public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
  {
    var cart = await repository.StoreBasket(command.Cart, cancellationToken);

    return new StoreBasketResult(cart.UserName);
  }
}
