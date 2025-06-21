using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]//Override route if needed

[ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(Result), StatusCodes.Status409Conflict)]

public class ApiBaseController : ControllerBase
{
}
