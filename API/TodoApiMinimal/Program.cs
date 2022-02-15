using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(options => options.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/todoitems", async (TodoDb db) => 
    await db.Todos
        .Select(x => new TodoDTO(x))
        .ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos
        .Where(x => x.IsComplete)
        .Select(x => new TodoDTO(x))
        .ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo 
            ? Results.Ok(new TodoDTO(todo)) 
            : Results.NotFound());

app.MapPost("/todoitems", async (TodoDTO todoDTO, TodoDb db) =>
{    
    var todo = new Todo();
    todo.Name = todoDTO.Name;
    todo.IsComplete = todoDTO.IsComplete;
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, TodoDTO inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) 
    {
        return Results.NotFound();
    }
    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});

app.Run();

class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string Secret { get; set; } = Guid.NewGuid().ToString();
}

class TodoDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public TodoDTO() {}
    public TodoDTO(Todo todo) => (Id, Name, IsComplete) = (todo.Id, todo.Name, todo.IsComplete);
}

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) {}
    public DbSet<Todo> Todos => Set<Todo>();
}