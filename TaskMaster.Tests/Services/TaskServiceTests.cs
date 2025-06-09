using Bogus;
using FluentAssertions;
using TaskMaster.Data;
using TaskMaster.Dtos.TaskDTOs;
using TaskMaster.Entities;
using TaskMaster.Entities.Enum;
using TaskMaster.Services.TaskServices;
using TaskMaster.Tests.Helpers;
using TaskMaster.Validator.TaskValidator;
using CreateTaskValidator = TaskMaster.Validator.TaskValidator.CreateTaskValidator;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.Tests.Services;

public class TaskServiceTests : IClassFixture<TestFixture>
{
    private readonly AppDbContext _context;
    private readonly TaskService _taskService;
    private readonly CreateTaskValidator _validator;
    private readonly Faker _faker = new();
    private readonly TestFixture _fixture;

    public TaskServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.Context;
        _taskService = new TaskService(_context);
        _validator = new CreateTaskValidator();
    }

    [Fact]
    public void Should_Have_Errors_When_All_Fields_Are_Invalid()
    {
        var model = new CreateTaskDTO("", "", 0, (Priority)5);
        var result = _validator.Validate(model);
        result.IsValid.Should().BeFalse();

        var expectedMessages = new[]
        {
            "Title cannot be empty",
            "Description cannot be empty",
            "Priority invalid",
            "UserId cannot be empty"
        };
        var actualMessages = result.Errors.Select(x => x.ErrorMessage);
        actualMessages.Should().BeEquivalentTo(expectedMessages);
    }

    [Fact]
    public async Task Should_Exist_Same_Task_In_Database()
    {
        
        var duplicateTask = new CreateTaskDTO(
            _fixture.DefaultTask.Title,
            "Outra descrição",
            _fixture.DefaultUser.Id,
            Priority.High
        );
        
        Func<Task> act = async () => await _taskService.CreateTask(duplicateTask);
        
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Task with the same Title already exists");
    }

    [Fact]
    public async Task Should_User_Not_Exist_In_Database()
    {
        var duplicateTask = new CreateTaskDTO(
            "Titulo Test",
            "Outra descrição",
            99999,
            Priority.High
        );
        Func<Task> act = async () => await _taskService.CreateTask(duplicateTask);
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("User with this id doesn't exist");
    }

    [Fact]
    public async Task Should_Create_Task_Successfully()
    {
      
        var user = new User("João", "joao@email.com", "senha123");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var dto = new CreateTaskDTO(_faker.Lorem.Text(), _faker.Lorem.Paragraph(), user.Id, _faker.Random.Enum<Priority>());

       
        var result = await _taskService.CreateTask(dto);

      
        result.Title.Should().Be(dto.Title);
        result.Description.Should().Be(dto.Description);
    }

    [Fact]
    public async Task Should_Throw_task_NotFound()
    {
        var id = 2;
        Func<Task> act = async () => await _taskService.GetTaskOrThrow(id);
        
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Task with id {id} not found");
    }

    [Fact]
    public async Task Should_Get_Task_by_id()
    {
        var id = 1;
        var result = await _taskService.GetTaskOrThrow(id);
        result.Id.Should().Be(id);
    }
    
    [Fact]
    public async Task GetTaskById_Should_Return_TaskDTO()
    {
        var task = _fixture.DefaultTask;
        var result = await _taskService.GetTaskById(task.Id);
    
        result.Should().NotBeNull();
        result.Id.Should().Be(task.Id);
        result.Title.Should().Be(task.Title);
    }

    [Fact]
    public async Task GetTasks_Should_Return_All_Tasks()
    {
        var newTask = new CreateTaskDTO(
            "Titulo Test",
            "Outra descrição",
            1,
            Priority.High
        );
        
         await _taskService.CreateTask(newTask);
         
         var result = await _taskService.GetTasks();

         result.Should().NotBeNull();
         result.Should().HaveCount(2);
    }
    
    
    //Update Task Tests
    [Fact]
    public void Update_Task_Should_Request_Invalid()
    {
        var title = "";
        var description = "";
        var status = 5;
        var priority = 8;
        
        var taskInvalid = new UpdateTaskDTO(title, description, (Status)status, (Priority)priority);
        
        var validator = new UpdateTaskValidator();
        
        var result = validator.Validate(taskInvalid);
        
        var expectedMessages = new[]
        {
            "Title cannot be empty",
            "Description cannot be empty",
            "Priority invalid",
            "Status invalid"
        };
        var actualMessages = result.Errors.Select(x => x.ErrorMessage);
        actualMessages.Should().BeEquivalentTo(expectedMessages);
        
    }
    [Fact]
    public async Task Update_Task_should_Sucessfully()
    {
        var title = _faker.Lorem.Text();
        var description = _faker.Lorem.Paragraph();
        var priority = _faker.Random.Enum<Priority>();
        var status = _faker.Random.Enum<Status>();
        
        var updatedDto = new UpdateTaskDTO(title, description, status, priority);
        
        var taskId = _fixture.DefaultTask.Id;
        
        var updatedTask = await _taskService.UpdateTask(taskId, updatedDto);
        
        updatedTask.Title.Should().Be(title);
        updatedTask.Description.Should().Be(description);
        updatedTask.Status.Should().Be(status);
        updatedTask.Priority.Should().Be(priority);
     
        
        var taskInDb = await _context.Tasks.FindAsync(taskId);
        
        taskInDb.Title.Should().Be(title);
        taskInDb.Description.Should().Be(description);
        taskInDb.Status.Should().Be(status);
        taskInDb.Priority.Should().Be(priority);
    }

    [Fact]
    public async Task Delete_Task_should_Successfully()
    {
        var id = _fixture.DefaultTask.Id;
        var existingTask = await _context.Tasks.FindAsync(id);
        existingTask.Should().NotBeNull();
        
        await _taskService.DeleteTask(id);
        
        var result = await _context.Tasks.FindAsync(id);
        result.Should().BeNull();
    }
    
}
