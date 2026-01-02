using Microsoft.Data.Sqlite;

namespace ContactsManager.Tests;

public class DbContextFixture : IDisposable
{
    public SqliteConnection Connection { get; private set; }
    public DbContextFixture()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();
    }
    public void Dispose()
    {
        Connection.Close();
        Connection.Dispose();
    }
}
