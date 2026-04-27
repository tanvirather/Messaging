using Microsoft.EntityFrameworkCore;
using Zuhid.Base;
using Zuhid.Notification.Entities;

namespace Zuhid.Notification;

public class NotificationContext(DbContextOptions<NotificationContext> options) : DbContext(options)
{
    public virtual DbSet<AddressEntity> Address { get; set; }
    public virtual DbSet<CategoryEntity> Category { get; set; }
    public virtual DbSet<CustomerEntity> Customer { get; set; }
    public virtual DbSet<OrderDetailEntity> OrderDetail { get; set; }
    public virtual DbSet<OrderEntity> Order { get; set; }
    public virtual DbSet<ProductEntity> Product { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ToSnakeCase("notification");

        builder.LoadCsvData<AddressEntity>("Dataload/AddressEntity.csv");
        builder.LoadCsvData<CategoryEntity>("Dataload/CategoryEntity.csv");
        builder.LoadCsvData<CustomerEntity>("Dataload/CustomerEntity.csv");
        builder.LoadCsvData<OrderDetailEntity>("Dataload/OrderDetailEntity.csv");
        builder.LoadCsvData<OrderEntity>("Dataload/OrderEntity.csv");
        builder.LoadCsvData<ProductEntity>("Dataload/ProductEntity.csv");
    }
}
