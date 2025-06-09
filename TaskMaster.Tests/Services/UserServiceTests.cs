using Bogus;
using FluentAssertions;
using TaskMaster.Data;
using TaskMaster.Dtos.UserDTOs;
using TaskMaster.Exceptions;
using TaskMaster.Services.TaskServices;
using TaskMaster.Services.UserServices;
using TaskMaster.Tests.Helpers;
using TaskMaster.Validator.UserValidator;

namespace TaskMaster.Tests.Services;

public class UserServiceTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly AppDbContext _context;
    private readonly UserService _userService;
    private readonly CreateUserValidator  _validator;
    private readonly Faker _faker = new();

    public UserServiceTests(TestFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.Context;
        _userService = new UserService(_context);
        _validator = new CreateUserValidator();
    }

    [Fact]
    public void Should_Have_Errors_When_All_Fields_Are_Invalid()
    {
        var model = new CreateUserDTO("", "", "");
        var result = _validator.Validate(model);
        result.IsValid.Should().BeFalse();

        var expectedMessages = new[]
        {
            "Name cannot be empty",
            "Email cannot be empty",
            "Please enter a valid email address",
            "Password cannot be empty",
            "Password must have at least 6 characters"
        };
        var actualMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
        actualMessages.Should().BeEquivalentTo(expectedMessages);
    }

    [Fact]
    public async Task Should_Have_Error_If_User_With_Same_Email_Exist_In_Database()
    {
        var userDuplicated = new CreateUserDTO("Test", _fixture.DefaultUser.Email, "123456");
        Func<Task> act = async () => await _userService.CreateUser(userDuplicated);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("User with the same Email already exists");
    }

    [Fact]
    public async Task Should_Have_Create_User_Sucessfuly()
    {
        var user = new CreateUserDTO(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password());
        var result = await _userService.CreateUser(user);
        

        result.Email.Should().Be(user.Email);
    }
    [Fact]
    public async Task Should_Return_UserDTO_When_GetUserById_With_Valid_Id()
    {
        // Arrange
        var user = _fixture.DefaultUser;

        // Act
        var result = await _userService.GetUserById(user.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task Should_Throw_When_GetUserById_With_Invalid_Id()
    {
       
        var invalidId = 999;
        
        Func<Task> act = async () => await _userService.GetTaskOrThrow(invalidId);

     
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with id {invalidId} not found");
    }

    [Fact]
    public async Task Should_Return_Correct_UserDTO_Equivalent()
    {
       
        var user = _fixture.DefaultUser;

      
        var result = await _userService.GetUserById(user.Id);

   
        var expected = new UserDTO
        (
            user.Id,
            user.Name,
            user.Email
        );

        result.Should().BeEquivalentTo(expected);
    }
    [Fact]
    public async Task Should_Update_User_Successfully()
    {
        // Arrange
        var existingUser = _fixture.DefaultUser;
        var updateDto = new UpdateUserDTO(
            _faker.Person.FullName,
            _faker.Person.Email,
            _faker.Internet.Password());

        // Act
        var result = await _userService.UpdateUser(existingUser.Id, updateDto);

        // Assert
        result.Name.Should().Be(updateDto.Name);
        result.Email.Should().Be(updateDto.Email);
    }
    [Fact]
    public async Task Should_Throw_When_Updating_NonExistent_User()
    {
        var updateDto = new UpdateUserDTO("Name", "test@email.com", "123456");

        Func<Task> act = async () => await _userService.UpdateUser(-999, updateDto);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User with id -999 not found");
    }
    [Fact]
    public async Task Should_Throw_When_Update_With_Invalid_Data()
    {
        var existingUser = _fixture.DefaultUser;
        var invalidDto = new UpdateUserDTO("", "invalidemail", "");

        Func<Task> act = async () => await _userService.UpdateUser(existingUser.Id, invalidDto);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }
    [Fact]
    public async Task Should_Throw_When_Email_Already_Exists_On_Update()
    {
        var existingUser = _fixture.DefaultUser;
        
        var anotherUser = new CreateUserDTO("Other", "duplicate@email.com", "123456");
        await _userService.CreateUser(anotherUser);

        var updateDto = new UpdateUserDTO("Updated Name", "duplicate@email.com", "123456");

        Func<Task> act = async () => await _userService.UpdateUser(existingUser.Id, updateDto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("User with the same Email already exists");
    }
    [Fact]
    public async Task Should_Delete_User_Successfully()
    {
        // Arrange
        var user = new CreateUserDTO("To Delete", _faker.Person.Email, "123456");
        var created = await _userService.CreateUser(user);

        // Act
        await _userService.DeleteUser(created.Id);

        // Assert
        var act = async () => await _userService.GetUserById(created.Id);
        await act.Should().ThrowAsync<ArgumentException>();
    }
    [Fact]
    public async Task Should_Throw_When_Deleting_User_With_Tasks()
    {
        var user = _fixture.DefaultUser;

        Func<Task> act = async () => await _userService.DeleteUser(user.Id);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Não é possível deletar o usuário porque ele possui tarefas associadas.");
    }
}