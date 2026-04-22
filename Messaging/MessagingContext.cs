using Microsoft.EntityFrameworkCore;
using Zuhid.Base;
using Zuhid.Messaging.Entities;

namespace Zuhid.Messaging;

public class MessagingContext(DbContextOptions<MessagingContext> options) : DbContext(options)
{
    public virtual DbSet<OrderEntity> Order { get; set; }
    public virtual DbSet<OrderDetailEntity> OrderDetail { get; set; }
    public virtual DbSet<CategoryEntity> Category { get; set; }
    public virtual DbSet<ProductEntity> Product { get; set; }
    public virtual DbSet<CustomerEntity> Customer { get; set; }
    public virtual DbSet<AddressEntity> Address { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ToSnakeCase("messaging");

        builder.LoadCsvData<CategoryEntity>("Dataload/CategoryEntity.csv");
        builder.LoadCsvData<ProductEntity>("Dataload/ProductEntity.csv");
        builder.LoadCsvData<CustomerEntity>("Dataload/CustomerEntity.csv");
        builder.LoadCsvData<AddressEntity>("Dataload/AddressEntity.csv");
        builder.LoadCsvData<OrderEntity>("Dataload/OrderEntity.csv");
        builder.LoadCsvData<OrderDetailEntity>("Dataload/OrderDetailEntity.csv");
    }
}
