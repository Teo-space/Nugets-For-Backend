using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers;


[Route("api/[controller]")]//Override route if needed
[ProducesResponseType(typeof(Result<HttpStatusCode>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(Result<HttpStatusCode>), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(Result<HttpStatusCode>), StatusCodes.Status409Conflict)]
[ApiController]
public class ApiBaseController : ControllerBase
{
}
