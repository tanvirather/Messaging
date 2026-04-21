using Microsoft.EntityFrameworkCore;
using Zuhid.Base;
using Zuhid.Messaging.Entities;

namespace Zuhid.Messaging;

public class MessagingContext(DbContextOptions<MessagingContext> options) : DbContext(options)
{
    public virtual DbSet<OrderEntity> Order { get; set; }
    public virtual DbSet<OrderDetailEntity> OrderDetail { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ToSnakeCase("messaging");
    }
}
