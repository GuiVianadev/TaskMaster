namespace TaskMaster.Entities;

public class User
{
    public int Id { get; init; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; init; } 
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Entities.Task> Tasks { get; set; }

    public User() { }

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
        Tasks = new List<Entities.Task>();
    }
    
    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
    }

    public void UpdatePassword(string password)
    {
        Password = password;
    }
}