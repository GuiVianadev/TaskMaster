using Bogus;
using FluentAssertions;

namespace TaskMaster.Tests.Entities;

using TaskMaster.Entities.Enum;
using Xunit;
using Task = TaskMaster.Entities.Task;

public class TaskTests
{
    private readonly Faker  _faker = new Faker();
    /* [Fact]
     public void Constructor_GivenAllParameters_ThenShouldSetThePropertiesCorrectly()
     {
         //Arrange
         var expectedTitle = "Title test";
         var expectedDescription = "Description test";
         var expectedPriority = Priority.Low;

         //Act
         var task = new Task(expectedTitle, expectedDescription, expectedPriority);
         //Assert
         Assert.Equal(expectedTitle, task.Title);
         Assert.Equal(expectedDescription, task.Description);
         Assert.Equal(expectedPriority, task.Priority);
     }
     */
    /*
    [Theory]
    [InlineData("Title test","Description test", Priority.Low)]
    [InlineData("Title test2","Description test2", Priority.Medium)]
    [InlineData("Title test3","Description test3", Priority.High)]
    public void Constructor_GivenAllParameters_ThenShouldSetThePropertiesCorrectly(
        string expectedTitle, string expectedDescription, Priority expectedPriority)
    {

        //Act
        var task = new Task(expectedTitle, expectedDescription, expectedPriority);
        //Assert
        Assert.Equal(expectedTitle, task.Title);
        Assert.Equal(expectedDescription, task.Description);
        Assert.Equal(expectedPriority, task.Priority);
    }
    */
    /*[Fact]
    public void Constructor_GivenAllParameters_ThenShouldSetThePropertiesCorrectly()
    {
        var expectedTitle = _faker.Random.Word();
        var expectedDescription = _faker.Random.Word();
        var expectedPriority = _faker.Random.Enum<Priority>();
        //Act
        var task = new Task(expectedTitle, expectedDescription, expectedPriority);
        //Assert
        Assert.Equal(expectedTitle, task.Title);
        Assert.Equal(expectedDescription, task.Description);
        Assert.Equal(expectedPriority, task.Priority);
    }*/
    
    [Fact]
    public void Constructor_GivenAllParameters_ThenShouldSetThePropertiesCorrectly()
    {
        var expectedTitle = _faker.Random.Word();
        var expectedDescription = _faker.Random.Word();
        var expectedPriority = _faker.Random.Enum<Priority>();
        var expectedUserId = _faker.Random.Int();
           
        var task = new Task(expectedTitle, expectedDescription, expectedPriority, expectedUserId);
           
       task.Title.Should().Be(expectedTitle);
       task.Description.Should().Be(expectedDescription);
       task.Priority.Should().Be(expectedPriority);
    }

    [Fact]
    public void Change_Title_GivenTaskWithTheSameTitle_ThenShouldSetThePropertiesCorrectly()
    {
       
        var title = _faker.Random.Word();
        var description = _faker.Random.Word();
        var priority = _faker.Random.Enum<Priority>();
        var userId = _faker.Random.Int();
        
        var task = new Task(title, description, priority, userId);

        var newTitle = _faker.Random.Word();

       
        task.UpdateTitle(newTitle);

      
        task.Title.Should().Be(newTitle);
    }
    [Fact]
    public void Change_Description_GivenTaskWithTheSameTitle_ThenShouldSetThePropertiesCorrectly()
    {
        
        var title = _faker.Random.Word();
        var description = _faker.Random.Word();
        var priority = _faker.Random.Enum<Priority>();
        var userId = _faker.Random.Int();
        var task = new Task(title, description, priority, userId);

        var newDescription = _faker.Random.Word();

        
        task.UpdateDescription(newDescription);
        
        task.Description.Should().Be(newDescription);
    }
    
    [Fact]
    public void Change_Priority_GivenTaskWithTheSameTitle_ThenShouldSetThePropertiesCorrectly()
    {
        
        var title = _faker.Random.Word();
        var description = _faker.Random.Word();
        var priority = _faker.Random.Enum<Priority>();
        var userId = _faker.Random.Int();
        var task = new Task(title, description, priority,  userId);

        var newPriority = _faker.Random.Enum<Priority>();

        
        task.UpdatePriority(newPriority);
        
        task.Priority.Should().Be(newPriority);
    }
}