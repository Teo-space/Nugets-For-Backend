using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public partial record Result<T> 
{
    public static implicit operator ActionResult(Result<T> result) => result.ToActionResult();

    public new ActionResult ToActionResult()
    {
        return this switch
        {
            { Type: Statuses.Ok } => new OkObjectResult(this.Value),

            { Type: Statuses.NoContent } => new NoContentResult(),

            { Type: Statuses.BadRequest } => new BadRequestObjectResult(this.ToErrorResult()),

            { Type: Statuses.NotFound } => new NotFoundObjectResult(this.ToErrorResult()),

            { Type: Statuses.Conflict } => new ConflictObjectResult(this.ToErrorResult()),

            { Type: Statuses.Deleted } => new ObjectResult(this.ToErrorResult())
            {
                StatusCode = StatusCodes.Status410Gone
            },

            { Type: Statuses.Unauthorized } => new UnauthorizedObjectResult(this.ToErrorResult()),

            { Type: Statuses.NotEnoughPermissions } => new ObjectResult(this.ToErrorResult())
            {
                StatusCode = StatusCodes.Status403Forbidden,
            },

            { Type: Statuses.InternalServerError } => new ObjectResult(this.ToErrorResult())
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            },

            _ => throw new NotImplementedException()
        };
    }
}
