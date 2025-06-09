using TaskMaster.Dtos.TaskDTOs;
using TaskMaster.Dtos.UserDTOs;

namespace TaskMaster.Services.UserServices;

public interface IUserService
{
    System.Threading.Tasks.Task<UserDTO> CreateUser(CreateUserDTO request);
    System.Threading.Tasks.Task<UserDTO> GetUserById(int id);
    System.Threading.Tasks.Task<IEnumerable<UserDTO>> GetUsers(); 
    System.Threading.Tasks.Task<UserDTO> UpdateUser(int id, UpdateUserDTO request);
    System.Threading.Tasks.Task DeleteUser(int id);
}