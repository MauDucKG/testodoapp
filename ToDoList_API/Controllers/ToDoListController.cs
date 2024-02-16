using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList_API.Data;
using ToDoList_API.Models;

namespace ToDoList_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly ToDoListApiDbContext dbContext;

        public ToDoListController(ToDoListApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetToDoLists([FromQuery]string? filterOn, [FromQuery] string? filterText)
        {
            if(filterOn is not null && filterText is not null)
            {
                if(filterOn.Equals("status",StringComparison.OrdinalIgnoreCase))
                {
                    var filterStatus = (Status)Enum.Parse(typeof(Status), filterText);

                    var ToDoLists = await dbContext.ToDoLists
                        .Where(x => x.Status == filterStatus)
                        .ToListAsync();

                    return Ok(ToDoLists);
                }
            }
            return Ok(await dbContext.ToDoLists.ToListAsync());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetToDoList([FromRoute] Guid id)
        {
            var todolist = await dbContext.ToDoLists.FindAsync(id);
            if (todolist == null)
                return NotFound();
            return Ok(todolist);
        }
        [HttpPost]
        public async Task<IActionResult> AddToDoList(AddToDoListRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ToDoList = new ToDoList()
            {
                ID = Guid.NewGuid(),
                Category = request.Category,
                DueDate = request.DueDate,
                EstimatedTime = request.EstimatedTime,
                Priority = request.Priority,
                Status = request.Status,
                Title = request.Title,
                UserID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
            };
            await dbContext.ToDoLists.AddAsync(ToDoList);
            await dbContext.SaveChangesAsync();
            return Ok(ToDoList);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateToDoList([FromRoute] Guid id, UpdateToDoListRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todolist = await dbContext.ToDoLists.FindAsync(id);
            if (todolist == null)
                return NotFound();
            todolist.DueDate = request.DueDate;
            todolist.Category = request.Category;
            todolist.EstimatedTime = request.EstimatedTime;
            todolist.Priority = request.Priority;
            todolist.Title = request.Title;
            todolist.Status = request.Status;
            todolist.UserID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await dbContext.SaveChangesAsync();
            return Ok(todolist);
        }

        private Guid GetUserID()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteToDoList([FromRoute] Guid id)
        {
            var todolist = await dbContext.ToDoLists.FindAsync(id);
            if (todolist == null)
                return NotFound();
            dbContext.ToDoLists.Remove(todolist);
            await dbContext.SaveChangesAsync();
            return Ok(todolist);
        }
    }
}
