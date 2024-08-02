using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public readonly partial record struct Result<T>
{
    public static implicit operator ActionResult(Result<T> result)
    {
        return result switch
        {
           { Type: Statuses.Ok } => new OkObjectResult(result.Value),
           { Type: Statuses.NoContent } => new NoContentResult(),
           { Type: Statuses.BadRequest } => new BadRequestObjectResult(result),
           { Type: Statuses.NotFound } => new NotFoundObjectResult(result),
           { Type: Statuses.ParentNotFound } => new NotFoundObjectResult(result),
           { Type: Statuses.Conflict } => new ConflictObjectResult(result),
           { Type: Statuses.Deleted } => new ObjectResult(result)
           {
               StatusCode = StatusCodes.Status410Gone
           },
           { Type: Statuses.Unauthorized } => new UnauthorizedObjectResult(result),
           { Type: Statuses.NotEnoughPermissions } => new ObjectResult(result)
           {
               StatusCode = StatusCodes.Status403Forbidden,
           },
           { Type: Statuses.InternalServerError } => new ObjectResult(result)
           {
               StatusCode = StatusCodes.Status500InternalServerError,
           },


           _ => throw new NotImplementedException()
        };

    }

}