using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public interface ITodoData
{
    Task Complete(int assignedTo, int todoId);
    Task<TodoModel?> Create(int assignedTo, string task);
    Task Delete(int assignedTo, int todoId);
    Task<List<TodoModel>> GetAllAssigned(int assignedTo);
    Task<TodoModel?> GetAssignedById(int assignedTo, int todoId);
    Task UpdateTask(int assignedTo, int todoId, string task);
}