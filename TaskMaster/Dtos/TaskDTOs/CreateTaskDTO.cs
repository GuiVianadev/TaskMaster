using System.Text.Json.Serialization;
using TaskMaster.Entities.Enum;

namespace TaskMaster.Dtos.TaskDTOs;

public record CreateTaskDTO(
     string Title,
     string Description,
     int UserId,
     Priority Priority
    );