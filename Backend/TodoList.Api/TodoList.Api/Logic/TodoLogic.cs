using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TodoList.Api.Interface;
using System.Reflection;


namespace TodoList.Api.Logic
{
    public class TodoLogic : ITodoLogic
    {
        readonly ITodoDataAccess _dataAccess;
        private readonly ILogger<TodoLogic> _logger;
        public TodoLogic(ITodoDataAccess dataAccess, ILogger<TodoLogic> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }
        public Task<TodoItemEntity> Add(TodoItemEntity todoItem)
        {
           try
            {
                if (todoItem.Id == Guid.Empty)
                {
                    todoItem.Id = Guid.NewGuid();
                }
               return _dataAccess.Add(todoItem);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{0} failed due to {1} ", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw;
            }
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TodoItemEntity> Get(Guid id)
        {
            return await _dataAccess.Get(id);
        }

        public async Task<IEnumerable<TodoItemEntity>> GetAll()
        {
            return await _dataAccess.GetAll();
        }

        public async Task<bool> TodoItemDescriptionExists(string description)
        {
            return await _dataAccess.TodoItemDescriptionExists(description);
        }

        public async Task<bool> TodoItemIdExists(Guid id)
        {
            return await _dataAccess.TodoItemIdExists(id);
        }

        public async Task Update(Guid id, TodoItemEntity todoItem)
        {
            try
            {
                if (id != todoItem.Id)
                {
                    return;
                }
                await _dataAccess.Update(id, todoItem);                
            }
            catch(Exception ex)
            {
                _logger.LogError($"{0} failed due to {1} ", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw;
            }
        }
    }
}
