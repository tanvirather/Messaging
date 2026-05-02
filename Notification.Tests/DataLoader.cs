using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Globalization;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests;

public class DataLoader
{
    public DataLoader()
    {
        MockContext = new Mock<NotificationContext>(new DbContextOptions<NotificationContext>());
    }

    public NotificationContext Context => MockContext.Object;

    public Mock<NotificationContext> MockContext { get; }

    public static List<T> LoadCsv<T>(string filename)
    {
        var csvPath = Path.Combine("Dataload", filename);
        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });
        return [.. csv.GetRecords<T>()];
    }

    public DataLoader SetupDbSet<T>(List<T> data, System.Linq.Expressions.Expression<Func<NotificationContext, DbSet<T>>> setupExpression) where T : class
    {
        MockContext.Setup(setupExpression).ReturnsDbSet(data);
        return this;
    }
}
