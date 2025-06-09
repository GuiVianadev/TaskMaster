using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Data;

namespace TaskMaster.Tests.Helpers;

public class DbContextHelper
{
    public static AppDbContext CreateContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}