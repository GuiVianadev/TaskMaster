using TaskMaster.Dtos.UserDTOs;
using TaskMaster.Entities;

namespace TaskMaster.Mappers;

public class UserMapper
{
    public static UserDTO ToUserDto(User user)
    {
        return new UserDTO(user.Id, user.Name, user.Email);
    }
}