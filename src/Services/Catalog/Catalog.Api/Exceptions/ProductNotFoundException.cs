namespace Catalog.Api.Exceptions;

public class ProductNotFoundException(Guid id)
  : Exception($"Product with Id {id} was not found.")
{
  public Guid Id { get; } = id;
}
