using MediatR;

namespace Catalog.Api.Products.CreateProduct;

public record CreateProductCommand(
  string Name,
  string Description,
  decimal Price,
  List<string> Category,
  string ImageFile
)
  : IRequest<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
  public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
  {
    // business logic to create a product
    throw new NotImplementedException();
  }
}