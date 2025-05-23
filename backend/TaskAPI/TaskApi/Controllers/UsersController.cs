using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.Models;
using TaskApi.Dtos;

namespace TaskApi.Controllers
{
    [ApiController] // define que aqui vão os controllers para o entity
    [Route("api/[controller]")] // convenção
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context) => _context = context;

        // GET: api/Users
        // faz uma requisição e retorna todos os usuários da api
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
            => await _context.Users // traz os usuários
                             .Include(u => u.Tasks) // inclui as tasks do usuário
                             .ToListAsync();

        // GET: api/Users/{id}
        [HttpGet("{id}")] // definindo o parametro id tipo um input
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var user = await _context.Users
                                     .Include(u => u.Tasks) // inclui as tarefas
                                     .FirstOrDefaultAsync(u => u.Id == id); // definindo para trazer o primeiro usuário
            if (user == null)
                return NotFound(); // se não achar retorna nao encontrado. NotFound vem do .net
            return user;
        }

        // POST: api/Users
        [HttpPost] // aqui vai o post
        public async Task<ActionResult<User>> Create([FromBody] User user) // puxa as infos do JSON e cria um body e transforma em um User do tipo User
        {
            user.Id = Guid.NewGuid(); // cria novo id da biblioteca
            _context.Users.Add(user); // fala pro entity guardar o user
            await _context.SaveChangesAsync(); // espera acabar e joga no banco

            return CreatedAtAction( 
                nameof(GetById),
                new { id = user.Id },
                user
            );
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User user) // faz um update onde o id corresponder e puxa as infos do body user
        {
            if (id != user.Id)
                return BadRequest();

            _context.Entry(user).State = EntityState.Modified; // se o usuário existe, modifica
            try
            {
                await _context.SaveChangesAsync(); // tenta salvar o JSON
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Users.AnyAsync(e => e.Id == id))
                    return NotFound(); // faz um teste para ver se o usuário nao foi apagado ou alterado no mesmo tempo
                throw;
            }
            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id); // ele espera encontrar o id
            if (user == null)
                return NotFound(); // se nao encontrar

            _context.Users.Remove(user); // se encontrar, remove
            await _context.SaveChangesAsync(); // salva
            return NoContent(); // 202
        }

        // POST: api/Users/{userId}/tasks
        [HttpPost("{userId}/tasks")]
        public async Task<ActionResult<TaskItem>> CreateTaskForUser(
            Guid userId,
            [FromBody] CreateTaskDto taskDto) // usa o intermediário
        {
            var user = await _context.Users.FindAsync(userId); // tenta achar pelo usuário
            if (user == null)
                return NotFound($"User {userId} not found."); // se nao encontrar

            var task = new TaskItem // cria a nova task nesse esqueleto inserindo o que está no JSON
            {
                Id = Guid.NewGuid(),
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Priority = taskDto.Priority,
                Status = taskDto.Status,
                UserId = userId
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync(); // espera salvar e add

            return CreatedAtAction( // devolve o objeto criado e add ao taskItems
                actionName: "GetById",
                controllerName: "TaskItems",
                routeValues: new { id = task.Id },
                value: task
            );
        }

        // PUT: api/Users/{userId}/tasks/{taskId}
        [HttpPut("{userId}/tasks/{taskId}")] // altera a tarefa pelo id
        public async Task<IActionResult> UpdateTaskForUser(
            Guid userId,
            Guid taskId,
            [FromBody] CreateTaskDto taskDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"User {userId} not found.");

            var task = await _context.TaskItems
                                     .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                return NotFound($"Task {taskId} not found for user {userId}.");

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Priority = taskDto.Priority;
            task.Status = taskDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.TaskItems.AnyAsync(t => t.Id == taskId && t.UserId == userId))
                    return NotFound();
                throw;
            }

            return NoContent(); // retorna 202 = sucesso
        }

        // DELETE: api/Users/{userId}/tasks/{taskId}
        [HttpDelete("{userId}/tasks/{taskId}")] // deleta a task do usuário
        public async Task<IActionResult> DeleteTaskForUser(Guid userId, Guid taskId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"User {userId} not found.");

            var task = await _context.TaskItems
                                     .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                return NotFound($"Task {taskId} not found for user {userId}.");

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
