namespace Ordering.Application.Exceptions;

public class OrderNotFoundException(Guid id)
  : NotFoundException("Order", id)
{
  public Guid Id { get; } = id;
}
