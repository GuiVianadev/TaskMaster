using Bogus;
using FluentAssertions;
using TaskMaster.Dtos.TaskDTOs;
using TaskMaster.Entities.Enum;
using TaskMaster.Validator.TaskValidator;

namespace TaskMaster.Tests.Validators;

public class CreateTaskValidatorTests
{
    private readonly CreateTaskValidator _validator;
    private readonly Faker  _faker = new("pt_BR");

    public CreateTaskValidatorTests()
    {
        _validator = new CreateTaskValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var model = new CreateTaskDTO(
            "", _faker.Lorem.Paragraph(),
            _faker.Random.Int(), _faker.Random.Enum<Priority>());
        
        var result = _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("Title cannot be empty");
    }
    
    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new CreateTaskDTO(
            _faker.Lorem.Word(), "",
            _faker.Random.Int(), _faker.Random.Enum<Priority>());
        
        var result = _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("Description cannot be empty");
    }
    [Fact]
    public void Should_Have_Error_When_Priority_Is_Invalid()
    {
        var model = new CreateTaskDTO(
            _faker.Lorem.Word(), _faker.Lorem.Paragraph(),
            _faker.Random.Int(), (Priority)999);
        
        var result = _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("Priority invalid");
    }
    
    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        var model = new CreateTaskDTO(
            _faker.Lorem.Word(), _faker.Lorem.Paragraph(),
            0, _faker.Random.Enum<Priority>());
        
        var result = _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("UserId cannot be empty");
    }
    
}