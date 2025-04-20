using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Management_Api.Interfaces;
using Task_Management_Api.Models;

namespace Task_Management_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var token = _loginService.Login(login);
                if (token != "unauthorized")
                {
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized();
                }
            }
            else { 
                return BadRequest(ModelState);
            }
           

        }

    }
}
