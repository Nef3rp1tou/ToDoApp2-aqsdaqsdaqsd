using Microsoft.EntityFrameworkCore;
using ToDoApp2.Models;

namespace ToDoApp2.Data;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions options) : base(options) { }

    public DbSet<TodoItem> Todos { get; set; }
}