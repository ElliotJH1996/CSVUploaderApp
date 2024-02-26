using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IO;
using System;
using Application.Core.Repositories.Users;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Application.Core.Models;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUsers _users;

        public UserController(ILogger<UserController> logger, IUsers users)
        {
            _logger = logger;
            _users = users;
        }

        [HttpPost]
        public IActionResult GetUser([FromBody] User userModel)
        {
            try
            {
                string username = userModel.Username;
                var base64EncodedBytes = Convert.FromBase64String(userModel.Password);
                string password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

				var user = _users.CheckUserCredentials(username, password);
                if (user == 0)
                {
                    return NotFound("This user does not exist");
                }

                return Ok("User exists");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Upload");
                return BadRequest("Error: " + e.Message);
            }
        }
    }
}
