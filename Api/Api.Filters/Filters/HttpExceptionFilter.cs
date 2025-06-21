using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;


namespace Api.Filters.Filters;


public class HttpExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<HttpExceptionFilter>>();

        if (context?.Exception is not null)
        {
            var exception = context.Exception;

            string exceptionType = exception.GetType().Name;

            var status = exception switch
            {
                //Пользователь авторизован, но нет прав для выполнения операции
                UnauthorizedAccessException => HttpStatusCode.Forbidden,

                //Объект не найден.
                KeyNotFoundApiException => HttpStatusCode.NotFound,

                //Ошибка ограничений бизнес логики.
                InvalidConstraintApiException => HttpStatusCode.Conflict,

                //Ошибка клиента
                InvalidOperationApiException => HttpStatusCode.BadRequest,


                Exception => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError,
            };


            context.ExceptionHandled = true;

            if (status == HttpStatusCode.Unauthorized)
            {
                context.ExceptionHandled = true;

                context.Result = Results.UnAuthorized($"[{exceptionType}] {exception.Message}").ToActionResult();

                logger.LogWarning(exception, $"Статус '{status}'. Произошла ошибка {{Exception}} {{Message}}", exceptionType, exception.Message);
            }
            else if (status == HttpStatusCode.Forbidden)
            {
                context.ExceptionHandled = true;

                context.Result = Results.NotEnoughPermissions($"[{exceptionType}] {exception.Message}").ToActionResult();

                logger.LogWarning(exception, $"Статус '{status}'. Произошла ошибка {{Exception}} {{Message}}", exceptionType, exception.Message);
            }
            else if (status == HttpStatusCode.NotFound)
            {
                context.ExceptionHandled = true;

                context.Result = Results.NotFound($"[{exceptionType}] {exception.Message}").ToActionResult();

                logger.LogWarning(exception, $"Статус '{status}'.Произошла ошибка {{Exception}} {{Message}}", exceptionType, exception.Message);
            }
            else if (status == HttpStatusCode.Conflict)
            {
                context.ExceptionHandled = true;

                context.Result = Results.Conflict($"[{exceptionType}] {exception.Message}").ToActionResult();

                logger.LogWarning(exception, $"Статус '{status}'.Произошла ошибка {{Exception}} {{Message}}", exceptionType, exception.Message);
            }
            else if (status == HttpStatusCode.BadRequest)
            {
                context.ExceptionHandled = true;

                context.Result = Results.Bad($"[{exceptionType}] {exception.Message}").ToActionResult();

                logger.LogWarning(exception, $"Статус '{status}'.Произошла ошибка {{Exception}} {{Message}}", exceptionType, exception.Message);
            }
            else if (status == HttpStatusCode.InternalServerError)
            {
                context.ExceptionHandled = true;

                context.Result = Results.Exception($"[{exceptionType}] {exception.Message}").ToActionResult();

                logger.LogWarning(exception, $"Статус '{status}'.Произошла ошибка {{Exception}} {{Message}}", exceptionType, exception.Message);
            }

        }
    }
}