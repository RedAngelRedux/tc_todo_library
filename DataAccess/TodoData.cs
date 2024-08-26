using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public class TodoData : ITodoData
{
    private readonly ISqlDataAccess _sql;

    public TodoData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    /// <summary>
    /// This method will create a single record in the Todos table
    /// </summary>
    /// <param name="assignedTo">User's Id</param>
    /// <param name="task">The descripiton of this to do</param>
    /// <returns>An object model of the newly created record, or null</returns>
    public async Task<TodoModel?> Create(int assignedTo, string task)
    {
        var results = await _sql.LoadData<TodoModel, dynamic>(
            "dbo.spTodos_Create",
            new { assignedTo = assignedTo, Task = task },
            "Default");

        return results.FirstOrDefault(); // The default of any object is null
    }

    public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
    {
        return _sql.LoadData<TodoModel, dynamic>(
            "dbo.spTodos_GetAllAssigned",
            new { AssignedTo = assignedTo },
            "Default");
    }

    public async Task<TodoModel?> GetAssignedById(int assignedTo, int todoId)
    {
        var results = await _sql.LoadData<TodoModel, dynamic>(
            "dbo.spTodos_GetAssignedById",
            new { AssignedTo = assignedTo, TodoId = todoId },
            "Default");

        return results.FirstOrDefault();
    }

    public Task UpdateTask(int assignedTo, int todoId, string task)
    {
        // This will do work after returning Task, but we don't have to await it here
        return _sql.SaveData<dynamic>(
            "dbo.spTodos_UpdateTask",
            new { AssignedTo = assignedTo, TodoId = todoId, Task = task },
            "Default");
    }

    public Task Complete(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spTodos_Complete",
            new { AssignedTo = assignedTo, TodoId = todoId },
            "Default");
    }

    public Task Delete(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spTodos_Delete",
            new { AssignedTo = assignedTo, TodoId = todoId },
            "Default");
    }
}
