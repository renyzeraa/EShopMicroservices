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

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
  public CreateProductCommandValidator()
  {
    RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required.");
    RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
  }
}

internal class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
  public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
  {
    logger.LogInformation("Creating product: {Name}", command.Name);

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