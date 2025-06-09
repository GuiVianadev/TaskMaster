using Bogus;
using FluentAssertions;
using TaskMaster.Dtos.UserDTOs;
using TaskMaster.Validator.UserValidator;

namespace TaskMaster.Tests.Validators;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator;
    private readonly Faker _faker = new("pt_BR");

    public CreateUserValidatorTests()
    {
        _validator = new CreateUserValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateUserDTO("", _faker.Person.Email,  _faker.Internet.Password());
        var result = _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("Name cannot be empty");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new CreateUserDTO(_faker.Person.FirstName, "", _faker.Internet.Password());
        var result =  _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("Email cannot be empty");
    }
    
    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new CreateUserDTO(_faker.Person.FirstName, "test", _faker.Internet.Password());
        var result =  _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("Please enter a valid email address");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new CreateUserDTO(_faker.Person.FirstName, _faker.Person.Email, "");
        var result =  _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("Password cannot be empty");
    }
    
    [Fact]
    public void Should_Have_Error_When_Password_Is_Less_than_six_caracters()
    {
        var model = new CreateUserDTO(_faker.Person.FirstName, _faker.Person.Email, "12345");
        var result =  _validator.Validate(model);
        
        result.IsValid.Should().BeFalse();
        result.Errors[0].ErrorMessage.Should().Be("Password must have at least 6 characters");
    }
    [Fact]
    public void Should_Have_Errors_When_All_Fields_Are_Invalid()
    {
        var model = new CreateUserDTO("", "", "");
        var result = _validator.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
    [Fact]
    public void Should_Not_Have_Errors_When_All_Fields_Are_Valid()
    {
        var model = new CreateUserDTO(
            _faker.Person.FirstName,
            _faker.Person.Email,
            _faker.Internet.Password(6)
        );

        var result = _validator.Validate(model);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}