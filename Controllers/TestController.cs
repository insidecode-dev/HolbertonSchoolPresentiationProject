using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolbertonExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult GetA()
        {
            return Ok("this endpoint can be used by admin");
        }

        [HttpGet("user")]
        [Authorize(Roles = "user")]
        public IActionResult GetU()
        {
            return Ok("this endpoint can be used by user");
        }

        [HttpGet("guest")]
        [Authorize(Roles = "guest")]
        public IActionResult GetG()
        {
            return Ok("this endpoint can be used by guest");
        }
    }
}
