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
                IReadOnlyCollection<string> errors = actionContext.ModelState
                    .Where(modelError => modelError.Value != null)
                    .Where(modelError => modelError.Value.Errors.Any())
                    .SelectMany(modelError => modelError.Value.Errors
                    .Select(x => $"{modelError.Key} : '{x.ErrorMessage}'"))
                    .Order()
                    .ToArray();

                return Results.Bad("Input Validation Error", errors).ToActionResult();
            };
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.MaxDepth = 10;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            options.JsonSerializerOptions.PropertyNamingPolicy = default;
        });

        return services;
    }


}
