using api.data;
using api.models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnection.GetConnectionString
("DefaultConnection") ?? throw new InvalidOperationException("Brak connetion stringa");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
 app.UseSwagger();
 app.UseSwaggerUI();   
}

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Message = "Hello from .NET in Docker!",
        Time = DateTime.UtcNow
    });
});

app.MapGet("/hello/{name}", (string name) =>
{
    return Results.Ok(new
    {
        Message = $"Hello {name} .NET in Docker!",
        Time = DateTime.UtcNow
    });
});

app.MapGet("/todos", async (AppDbContext db) =>
{
    var todos= await db.todos
    .OrderByDescending(t => t.CreatedAt)
    .ToListAsync();
    
});

app.MapGet("/todos/{id:int}", async (int id, AppDbContext db) =>
{
    var todo = await db.Todos.FindAsync(id);
    return todo is null ? Results.NotFound() : Results.Ok(todo);

});

app.MapPost("/todos", async (TodoItem dto, AppDbContext db) =>
{
    db.Todos.Add(dto);
    await db.Save.ChangesAsync();
    return Results.Created($"/todos/{dto.Id}", dto);
});

app.MapPut("/todos/{id:int}", async (int id, TodoItem dto, AppDbContext db) =>
{
var existing = await db. Todos.FindAsyne(id);
if (existing is null) return Results.NotFound();

existing. Title = dto. Title;
existing. IsDone = dto.IsDone;

await db.SaveChangesAsync();
return Results.NoContent();
});

app.MapGet("/todos/{id:int}", async (int id, AppDbContext db) =>
{
    var existing = await db.Todos.FindAsync(id);
    if (existing is null) return Results.NotFound();

    db.Todos.Remuve(existing);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.Run();
