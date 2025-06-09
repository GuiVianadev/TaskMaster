using TaskMaster.Entities.Enum;

namespace TaskMaster.Entities;

public class Task
{
    public int Id { get; init; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; }

    public int UserId { get; init; }
    public User User { get; init; }
    public Priority Priority { get; private set; }
    public DateTime CreateDate { get; init; }
    public DateTime? EndDate { get; private set; }

    private Task() { }

    public Task(string title, string description, Priority priority, int userId)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Status = Status.Active;
        CreateDate = DateTime.UtcNow;
        EndDate = null;
        UserId = userId;
    }
    
    public void UpdatePriority(Priority newPriority)
    {
        Priority = newPriority;
    }

    public void UpdateStatus(Status newStatus)
    {
        Status = newStatus;

        if (newStatus == Status.Completed)
        {
            EndDate = DateTime.UtcNow;
        }
        else
        {
            EndDate = null; 
        }
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
    }
}