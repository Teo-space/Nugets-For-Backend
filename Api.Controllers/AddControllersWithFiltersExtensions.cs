using Microsoft.AspNetCore.Mvc;
using System.Net;


public static class AddControllersWithFiltersExtensions
{
    public static IServiceCollection AddControllersWithFilters(this IServiceCollection services, params Type[] filters)
    {
        services.AddControllers(options =>
        {
            filters.ToList().ForEach(x => options.Filters.Add(x));
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = (actionContext) =>
            {
                var validationDetails = actionContext.ModelState
                    .Where(modelError => modelError.Value != null && modelError.Value.Errors.Any())
                    .Select(modelError => new FieldError()
                    {
                        FieldName = modelError.Key,
                        ErrorMessages = modelError.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                    }).ToArray();

                var result = Results.Errors<HttpStatusCode>(
                    Type: HttpStatusCode.BadRequest.ToString(), 
                    Detail: "ValidationErrors", 
                    errors: validationDetails);

                return new BadRequestObjectResult(result);
            };
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.MaxDepth = 10;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper;
            options.JsonSerializerOptions.PropertyNamingPolicy = default;
        });

        return services;
    }


}
