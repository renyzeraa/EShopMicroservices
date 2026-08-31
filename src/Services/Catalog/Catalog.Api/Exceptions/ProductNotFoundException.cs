namespace Catalog.Api.Exceptions;

public class ProductNotFoundException(Guid id)
  : NotFoundException("Product", id)
{
  public Guid Id { get; } = id;
}
