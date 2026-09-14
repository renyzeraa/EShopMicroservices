using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
	public DbSet<Coupon> Coupons => Set<Coupon>();

	public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
	{
	}
}