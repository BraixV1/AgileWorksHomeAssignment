using System.Net;
using System.Text;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers;
using Xunit.Abstractions;
using Guid = System.Guid;

namespace Tests;

public class TasksControllerTests
{
    private const string ConnectionString = "Data Source=../../../Tasks.db";

    private readonly AppDbContext _datastore;

    private readonly TasksController _controller;


    public TasksControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(ConnectionString)
            .Options;

        _datastore = new AppDbContext(options);
        _controller = new TasksController(_datastore);
    }

    [Fact]
    public async Task GetTests_Returns_All_Tests()
    {
        // Arrange
        var count = _datastore.ToDoTasks.Count();

        var MockTasks = new List<ToDoTask>()
        {
            new()
            {
                Description = "Test1", HasToBeDoneAtDt = DateTime.Parse("21-12-2004"),
                CreatedAtDt = DateTime.Parse("04-12-2004")
            },
            new()
            {
                Description = "Test2", HasToBeDoneAtDt = DateTime.Parse("05-07-2044"),
                CreatedAtDt = DateTime.Parse("01-12-2003"), CompletedAtDt = DateTime.Parse("01-12-2024")
            },
            new()
            {
                Description = "Test3", HasToBeDoneAtDt = DateTime.Parse("02-03-2022"),
                CreatedAtDt = DateTime.Parse("04-12-2007")
            }
        };

        foreach (var task in MockTasks) _datastore.Add(task);
        await _datastore.SaveChangesAsync();


        // Act
        var result = await _controller.GetAllTasks();


        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tasks = Assert.IsAssignableFrom<IEnumerable<ToDoTask>>(okResult.Value);
        Assert.Equal(count + 3, tasks.Count());
    }

    [Fact]
    public async Task AddTask_New_Task_Success()
    {
        // Arrange
        var task = new ToDoTask
        {
            Description = "Test1", HasToBeDoneAtDt = DateTime.Parse("21-12-2004"),
            CreatedAtDt = DateTime.Parse("04-12-2004")
        };

        // Act
        var result = await _controller.AddTask(task);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var taskReceived = Assert.IsAssignableFrom<ToDoTask>(okResult.Value);
        Assert.Equal(task.Id, taskReceived.Id);
    }

    [Fact]
    public async Task AddTask_Failure()
    {
        // Arrange
        var task = new ToDoTask
        {
            Description = "Test1"
        };


        // Act
        var result = await _controller.AddTask(task);


        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateTask_Success()
    {
        // Arrange
        var task = new ToDoTask
        {
            Description = "Test2",
            HasToBeDoneAtDt = DateTime.Parse("05-07-2044"),
            CreatedAtDt = DateTime.Parse("01-12-2003"),
            CompletedAtDt = DateTime.Parse("01-12-2024"),
            Id = Guid.NewGuid()
        };

        // Add the task
        await _datastore.AddAsync(task);
        await _datastore.SaveChangesAsync();

        var updatedTask = new ToDoTask
        {
            Description = "Test2 Edited",
            HasToBeDoneAtDt = DateTime.Parse("05-07-2043"),
            CreatedAtDt = DateTime.Parse("01-12-2003"),
            CompletedAtDt = DateTime.Parse("01-12-2024"),
            Id = task.Id
        };

        // Act
        var response = await _controller.UpdateTask(updatedTask);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response.Result);
        var updatedTaskResult = Assert.IsType<ToDoTask>(okResult.Value);
        Assert.Equal(updatedTask.Description, updatedTaskResult.Description);
        Assert.Equal(updatedTask.HasToBeDoneAtDt, updatedTaskResult.HasToBeDoneAtDt);
        Assert.Equal(updatedTask.CreatedAtDt, updatedTaskResult.CreatedAtDt);
        Assert.Equal(updatedTask.CompletedAtDt, updatedTaskResult.CompletedAtDt);
        Assert.Equal(updatedTask.Id, updatedTaskResult.Id);
    }

    [Fact]
    public async Task UpdateTask_Failure()
    {
        // Arrange
        var task = new ToDoTask
        {
            Description = "Test3 Edited",
            HasToBeDoneAtDt = DateTime.Parse("05-07-2043"),
            CreatedAtDt = DateTime.Parse("01-12-2003"),
            CompletedAtDt = DateTime.Parse("01-12-2024"),
            Id = Guid.NewGuid()
        };

        // Act
        var response = await _controller.UpdateTask(task);

        // Assert
        Assert.IsType<NotFoundObjectResult>(response.Result);
    }

    [Fact]
    public async Task CompleteTask_Success()
    {
        // Arrange
        var task = new ToDoTask
        {
            Description = "Test3 Edited",
            HasToBeDoneAtDt = DateTime.Parse("05-07-2043"),
            CreatedAtDt = DateTime.Parse("01-12-2003"),
            Id = Guid.NewGuid()
        };

        _datastore.Add(task);
        _datastore.SaveChanges();


        // Act
        var response = await _controller.CompleteTask(task.Id);
        var updated = await _datastore.ToDoTasks.FindAsync(task.Id);


        // Assert
        Assert.IsType<OkResult>(response);
        Assert.NotNull(updated);
        Assert.NotNull(updated.CompletedAtDt);
    }

    [Fact]
    public async Task CompleteTask_Failure()
    {
        // Arrange


        // Act
        var response = await _controller.CompleteTask(Guid.NewGuid());


        // Assert
        Assert.IsType<NotFoundObjectResult>(response);
    }

    [Fact]
    public async Task DeleteTask_Success()
    {
        // Arrange
        var task = new ToDoTask
        {
            Description = "Test3 Edited",
            HasToBeDoneAtDt = DateTime.Parse("05-07-2043"),
            CreatedAtDt = DateTime.Parse("01-12-2003"),
            Id = Guid.NewGuid()
        };

        _datastore.Add(task);
        _datastore.SaveChanges();

        // Act

        var response = await _controller.DeleteTask(task.Id);
        var check = await _datastore.ToDoTasks.FindAsync(task.Id);

        // Assert
        Assert.IsType<OkResult>(response);
        Assert.Null(check);
    }


    [Fact]
    public async Task DeleteTask_Failure()
    {
        // Arrange

        // Act

        var response = await _controller.DeleteTask(new Guid());

        // Assert
        Assert.IsType<NotFoundObjectResult>(response);
    }

    [Fact]
    public async Task GetTask_Success()
    {
        // Arrange
        var task = new ToDoTask
        {
            Description = "Test3 Edited",
            HasToBeDoneAtDt = DateTime.Parse("05-07-2043"),
            CreatedAtDt = DateTime.Parse("01-12-2003"),
            Id = Guid.NewGuid()
        };

        _datastore.Add(task);
        _datastore.SaveChanges();

        // Act
        var result = await _controller.GetTask(task.Id);


        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var taskResult = Assert.IsAssignableFrom<ToDoTask>(okResult.Value);
        Assert.NotNull(taskResult);
        Assert.Equal(task.Id, taskResult.Id);
        Assert.Equal(task.Description, taskResult.Description);
        Assert.Equal(task.CreatedAtDt, taskResult.CreatedAtDt);
        Assert.Equal(task.HasToBeDoneAtDt, taskResult.HasToBeDoneAtDt);
        Assert.Equal(task.CompletedAtDt, taskResult.CompletedAtDt);
    }

    [Fact]
    public async Task GetTask_Failure()
    {
        // Act
        var result = await _controller.GetTask(new Guid());


        // Assert
        var badResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.IsNotAssignableFrom<ToDoTask>(badResult.Value);
    }
}