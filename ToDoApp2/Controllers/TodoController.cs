using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp2.Data;
using ToDoApp2.Models;

namespace ToDoApp2.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoDbContext;

        public TodoController(TodoContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var todos = await _todoDbContext.Todos
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(todos);
        }

        [HttpGet]
        [Route("get-deleted-todos")]
        public async Task<IActionResult> GetDeletedTodos()
        {
            var todos = await _todoDbContext.Todos
                .Where(x => x.IsDeleted == true)
                .OrderByDescending(x => x.CreatedDate)  
                .ToListAsync();

            return Ok(todos);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodo(TodoItem todo)
        {
            todo.Id = Guid.NewGuid();

            _todoDbContext.Todos.Add(todo);
            await _todoDbContext.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] Guid id, TodoItem todoUpdateRequest)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);

            if(todo == null)
                return NotFound();

            todo.IsCompleted = todoUpdateRequest.IsCompleted;
            todo.CompletedDate = DateTime.Now;

            await _todoDbContext.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] Guid id)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);

            if (todo == null)
                return NotFound();

            todo.IsDeleted = true;
            todo.DeletedDate = DateTime.Now;

            await _todoDbContext.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpPut]
        [Route("undo-deleted-todo/{id:Guid}")]
        public async Task<IActionResult> UndoDeletedTodo([FromRoute] Guid id, TodoItem undoDeleteTodoRequest)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);

            if(todo == null)
                return NotFound();

            todo.DeletedDate = null;
            todo.IsDeleted = false;

            await _todoDbContext.SaveChangesAsync();

            return Ok(todo);
        }
    }
