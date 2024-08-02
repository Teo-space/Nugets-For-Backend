using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace Api.Headers;

public static class ApiHeadersExtensions
{
    /// <summary>
    /// Адаптированный набор прокинутых заголовков
    /// </summary>
    /// <param name="app">Контекст приложения</param>
    public static IApplicationBuilder UseIxForwardedHeaders(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        return app;
    }
}
