using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Data;
using TaskMaster.Dtos.UserDTOs;
using TaskMaster.Entities;
using TaskMaster.Exceptions;
using TaskMaster.Mappers;
using TaskMaster.Validator.UserValidator;
using Task = System.Threading.Tasks.Task;

namespace TaskMaster.Services.UserServices;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<UserDTO> CreateUser(CreateUserDTO request)
    {
        var validator = new CreateUserValidator();
        var result = validator.Validate(request);
        
        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
        
        var userExist = await _context.Users.AnyAsync(u => u.Email == request.Email);
        
        if (userExist)
        {
            throw new ArgumentException("User with the same Email already exists");
        }

      
        
        var user = new User(
                    request.Name,
                    request.Email,
                    request.Password);
        
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return UserMapper.ToUserDto(user);
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        var result = await _context.Users.FindAsync(id);
        if (result == null)
        {
            throw new ArgumentException("User with this id doesn't exist");
        }
        
        return UserMapper.ToUserDto(result);
    }

    public async Task<IEnumerable<UserDTO>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(UserMapper.ToUserDto);
    }

    public async Task<UserDTO> UpdateUser(int id, UpdateUserDTO request)
    {
        var user = await GetTaskOrThrow(id);
        
        var validator = new UpdateUserValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
        
        var userExist = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (userExist)
        {
            throw new ArgumentException("User with the same Email already exists");
        }
        
        user.UpdateName(request.Name);
        user.UpdateEmail(request.Email);
        user.UpdatePassword(request.Password);
        
        await _context.SaveChangesAsync();
        return UserMapper.ToUserDto(user);
    }

    public async Task DeleteUser(int id)
    {
        var user = await  GetTaskOrThrow(id);
        var hasTasks = await _context.Tasks.AnyAsync(t => t.UserId == id);
        if (hasTasks)
            throw new InvalidOperationException("Não é possível deletar o usuário porque ele possui tarefas associadas.");
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<User> GetTaskOrThrow(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
        return user;
    }
}