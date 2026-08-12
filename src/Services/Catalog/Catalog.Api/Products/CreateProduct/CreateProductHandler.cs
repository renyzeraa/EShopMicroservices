namespace Catalog.Api.Products.CreateProduct;

public record CreateProductCommand(
  string Name,
  string Description,
  decimal Price,
  List<string> Category,
  string ImageFile
)
  : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
  public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
  {
    var product = new Product
    {
      Name = command.Name,
      Category = command.Category,
      Description = command.Description,
      ImageFile = command.ImageFile,
      Price = command.Price
    };

    session.Store(product);
    await session.SaveChangesAsync(cancellationToken);

    return new CreateProductResult(product.Id);
  }
}