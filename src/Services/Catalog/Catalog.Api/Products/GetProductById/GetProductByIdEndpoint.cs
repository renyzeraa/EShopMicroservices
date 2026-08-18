namespace Catalog.Api.Products.GetProductById;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
    {
      var result = await sender.Send(new GetProductByIdQuery(id));

      if (result.Product is null)
        return Results.NotFound();

      var response = result.Adapt<GetProductByIdResponse>();

      return Results.Ok(response);
    })
    .WithName("GetProductById")
    .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .WithDescription("Get Product By Id")
    .WithSummary("Get Product By Id");
  }
}
