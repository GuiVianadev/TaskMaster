namespace TaskMaster.Dtos.UserDTOs;

public record class CreateUserDTO(
    string Name,
    string Email,
    string Password
    );
