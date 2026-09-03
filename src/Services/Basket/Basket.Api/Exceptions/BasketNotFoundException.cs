namespace Basket.Api.Exceptions;

public class BasketNotFoundException(string userName)
  : NotFoundException("Basket", userName)
{
  public string UserName { get; } = userName;
}
