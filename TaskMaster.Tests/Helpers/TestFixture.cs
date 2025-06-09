using Bogus;
using Microsoft.Data.Sqlite;
using TaskMaster.Data;
using TaskMaster.Entities;
using TaskMaster.Entities.Enum;

namespace TaskMaster.Tests.Helpers;

public class TestFixture : IDisposable
{
    private readonly SqliteConnection _connection;
    public AppDbContext Context { get; }
    public User DefaultUser { get; private set; }
    public TaskMaster.Entities.Task DefaultTask { get; private set; }

    public TestFixture()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        Context = DbContextHelper.CreateContext(_connection);
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var faker = new Faker("pt_BR");

        DefaultUser = new User(
            faker.Person.FullName,
            faker.Internet.Email(),
            faker.Internet.Password()
        );

        Context.Users.Add(DefaultUser);
        Context.SaveChanges();

        DefaultTask = new TaskMaster.Entities.Task(
            "Tarefa Exemplo",
            "Descrição de teste",
            Priority.Medium,
            DefaultUser.Id
        );

        Context.Tasks.Add(DefaultTask);
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Close(); 
        _connection.Dispose();
    }
}