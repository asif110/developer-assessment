using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Api.Interface
{
    public interface ITodoDataAccess
    {
        Task<IEnumerable<TodoItemEntity>> GetAll();
        Task<TodoItemEntity> Get(Guid id);
        Task Update(Guid id, TodoItemEntity todoItem);
        Task<TodoItemEntity> Add(TodoItemEntity todoItem);
        Task Delete(Guid id);
        Task<bool> TodoItemIdExists(Guid id);
        Task<bool> TodoItemDescriptionExists(string description);
    }
}
