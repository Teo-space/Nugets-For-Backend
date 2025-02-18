using Microsoft.AspNetCore.Mvc;

namespace Api.Tests.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(ILogger<TestController> logger, IConfiguration configuration) : ControllerBase
{

    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Test");


        return Ok(configuration.GetConnectionString("LOGS"));
    }
}
