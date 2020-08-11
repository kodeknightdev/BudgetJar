using System;
using Microsoft.AspNetCore.Mvc;

namespace BudgetJar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "ok", time = DateTime.UtcNow });
        }
    }
}
