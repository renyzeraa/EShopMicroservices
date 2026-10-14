namespace Shopping.Web.Models.Ordering;

public record PaginatedResult<TEntity>(int PageIndex, int PageSize, long Count, IEnumerable<TEntity> Data)
  where TEntity : class;
