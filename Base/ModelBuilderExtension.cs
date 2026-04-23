using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base;

public static class ModelBuilderExtension
{
    public static ModelBuilder ToSnakeCase(this ModelBuilder builder, string schema = "public")
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetSchema(entity.GetSchema() ?? schema); // Set schema if provided
            entity.SetTableName(entity.GetTableName()!.ToSnakeCase()); // Convert table name to snake_case
            entity.GetProperties().ToList().ForEach(property => property.SetColumnName(property.Name!.ToSnakeCase())); // Convert column names to snake_case
            entity.GetKeys().ToList().ForEach(key => key.SetName(key.GetName()?.ToLower())); // Convert key names to lower case
            entity.GetForeignKeys().ToList().ForEach(fk => fk.SetConstraintName(fk.GetConstraintName()?.ToLower())); // Convert foreign key names to lower case
            entity.GetIndexes().ToList().ForEach(index => index.SetDatabaseName(index.GetDatabaseName()?.ToLower())); // Convert index names to lower case
        }
        return builder;
    }

    public static void LoadCsvData<TEntity>(this ModelBuilder builder, string fileName) where TEntity : class
    {
        if (!EF.IsDesignTime)
        {
            return; // Only load CSV data during design time (e.g., migrations)
        }
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        var data = CsvSerializer.Load<TEntity>(filePath);
        builder.Entity<TEntity>().HasData(data);
    }
}
