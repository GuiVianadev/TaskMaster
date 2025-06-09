namespace TaskMaster.Dtos.UserDTOs;

public record UpdateUserDTO
(
    string Name,
    string Email,
    string Password
);