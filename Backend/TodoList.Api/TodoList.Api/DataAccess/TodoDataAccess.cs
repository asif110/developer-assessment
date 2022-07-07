using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TodoList.Api.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TodoList.Api.DataAccess
{
    public class TodoDataAccess : ITodoDataAccess
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoDataAccess> _logger;
        public TodoDataAccess(TodoContext context, ILogger<TodoDataAccess> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<TodoItemEntity> Add(TodoItemEntity todoItem)
        {
            try
            {
                todoItem = _context.TodoItems.Add(todoItem).Entity;
                await _context.SaveChangesAsync();

                return todoItem;
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
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<IEnumerable<TodoItemEntity>> GetAll()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public async Task<bool> TodoItemDescriptionExists(string description)
        {
            return await Task.FromResult(_context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted));
        }

        public async Task<bool> TodoItemIdExists(Guid id)
        {
            return await Task.FromResult( _context.TodoItems.Any(x => x.Id == id));
        }

        public async Task Update(Guid id, TodoItemEntity todoItem)
        {
            try
            {
                if (id != todoItem.Id)
                {
                    return;
                }
                _context.Entry(todoItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();                
            }
            catch(Exception ex)
            {
                _logger.LogError($"{0} failed due to {1} ", MethodBase.GetCurrentMethod().Name, ex.Message);
                throw;
            }
        }
    }
}
