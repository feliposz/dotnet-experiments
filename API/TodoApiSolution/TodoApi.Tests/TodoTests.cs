using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TodoApi.Data;
using TodoApi.Models;
using Xunit;

// Reference: https://github.com/davidfowl/CommunityStandUpMinimalAPI/blob/main/TodoApi.Tests/TodoTests.cs

namespace TodoApi.Tests;

public class TodoTests
{
    [Fact]
    public async Task GetTodos()
    {
        await using var application = new TodoApplication();

        var client = application.CreateClient();
        var todos = await client.GetFromJsonAsync<List<TodoItemDTO>>("/api/todoitems");

        Assert.Empty(todos);
    }

    [Fact]
    public async Task PostTodos()
    {
        await using var application = new TodoApplication();

        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/api/todoitems", new TodoItemDTO { Name = "I want to do this thing tomorrow" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var todos = await client.GetFromJsonAsync<List<TodoItemDTO>>("/api/todoitems");
        var todo = Assert.Single(todos);
        Assert.Equal("I want to do this thing tomorrow", todo.Name);
        Assert.False(todo.IsComplete);
    }

    [Fact]
    public async Task DeleteTodos()
    {
        await using var application = new TodoApplication();

        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/api/todoitems", new TodoItemDTO { Name = "This will be deleted", IsComplete = true });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var todos = await client.GetFromJsonAsync<List<TodoItemDTO>>("/api/todoitems");
        var todo = Assert.Single(todos);
        Assert.Equal("This will be deleted", todo.Name);
        Assert.True(todo.IsComplete);

        response = await client.DeleteAsync($"/api/todoitems/{todo.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await client.GetAsync($"/api/todoitems/{todo.Id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMissingTodo()
    {
        await using var application = new TodoApplication();

        var client = application.CreateClient();

        var response = await client.DeleteAsync($"/api/todoitems/0");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTodos()
    {
        await using var application = new TodoApplication();

        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync("/api/todoitems", new TodoItemDTO { Name = "This will be updated", IsComplete = false });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var todos = await client.GetFromJsonAsync<List<TodoItemDTO>>("/api/todoitems");
        var todo = Assert.Single(todos);
        Assert.Equal("This will be updated", todo.Name);
        Assert.False(todo.IsComplete);

        response = await client.PutAsJsonAsync($"/api/todoitems/{todo.Id}", new TodoItemDTO { Id = todo.Id, Name = "New content", IsComplete = true });
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        todo = await client.GetFromJsonAsync<TodoItemDTO>($"/api/todoitems/{todo.Id}");
        Assert.Equal("New content", todo?.Name);
        Assert.True(todo?.IsComplete);
    }

    [Fact]
    public async Task BadUpdateTodos()
    {
        await using var application = new TodoApplication();

        var client = application.CreateClient();
        var response = await client.PutAsJsonAsync($"/api/todoitems/1", new TodoItemDTO { Id = 2 });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        response = await client.PutAsJsonAsync($"/api/todoitems/0", new TodoItemDTO { Id = 0 });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

class TodoApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // NOTE: Actually the base implementation is already using an in memory database, 
        // but I kept this based on the reference, in case the implementation is changed.
        var root = new InMemoryDatabaseRoot();
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<TodoContext>));
            services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("Testing", root));
        });
        return base.CreateHost(builder);
    }
}