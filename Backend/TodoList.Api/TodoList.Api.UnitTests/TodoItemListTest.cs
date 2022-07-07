using Xunit;
using TodoList.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Interface;
using TodoList.Api.Logic;
using TodoList.Api.DataAccess;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;

namespace TodoList.Api.UnitTests
{
    //This test here are to illustrate the concept, we can extend this class to include many more tests and seperate out controller, service(logic) and dataaccess tests
    public class TodoItemListTest
    {
        private readonly TodoItemsController _controller;
        private readonly TodoLogic _logic;
        private readonly TodoDataAccess _dataAccess;
        private readonly TodoContext _context;
               

        public TodoItemListTest()
        {

            var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "TodoItemsDB")
            .Options;

            _context = new TodoContext(options);
            _dataAccess = new TodoDataAccess(_context, new NullLogger<TodoDataAccess>());
            _logic = new TodoLogic(_dataAccess, new NullLogger<TodoLogic>());
            _controller = new TodoItemsController(_logic);
        }

        
        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetTodoItems().Result;
            // Assert
             Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var testTodoItem = new TodoItemEntity
            {
                 Id =  new Guid("c9f0909e-bcb8-48a4-8a72-e256aca6b754"),
                Description = "item 1",
                IsCompleted = false
            };
            // Act
            var createdResponse = _controller.PostTodoItem(testTodoItem).Result;
            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        //Will pass only if Add_ValidObjectPassed_ReturnsCreatedResponse() has ran before
        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            var testTodoItem = new TodoItemEntity
            {
                Id = new Guid("aa8d0b60-4179-4e8d-b0ee-cee64ac7c316"),
                Description = "item 2",
                IsCompleted = false
            };
            // Act
            _= _controller.PostTodoItem(testTodoItem);
            // Act
            var okResult = _controller.GetTodoItems().Result as OkObjectResult;
            // Assert
            var items = Assert.IsType<List<TodoItemEntity>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }
    }
}

