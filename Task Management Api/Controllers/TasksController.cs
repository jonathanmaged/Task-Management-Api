using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Management_Api.Dtos;
using Task_Management_Api.Interfaces;
using Task_Management_Api.Shared;

namespace Task_Management_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto Dto)
        {
            if (ModelState.IsValid) 
            {
                var serviceresponse = await _taskService.CreateTaskAsync(Dto);

                if (serviceresponse.State == State.ServerError) { return StatusCode(500); }
                if (serviceresponse.State == State.NotFound) { return StatusCode(404,serviceresponse.Message); }

                return StatusCode(201,Dto);
                
            }
            else 
            {
                return BadRequest(ModelState);
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            if (ModelState.IsValid)
            {
                var serviceresponse = await _taskService.GetTaskByIdAsync(id);

                if (serviceresponse.State == State.NotFound) { return NotFound(); }

                return Ok(serviceresponse.Data);
            }
            return BadRequest(ModelState);
        }


        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByUser(int id)
        {
            if (ModelState.IsValid)
            {
                var serviceresponse = await _taskService.GetTasksByUserAsync(id);
                if (serviceresponse.State == State.NotFound) { return NotFound(serviceresponse.Message); }
                return Ok(serviceresponse.Data);                        
            }
            return BadRequest(ModelState);


        }
    }
}




