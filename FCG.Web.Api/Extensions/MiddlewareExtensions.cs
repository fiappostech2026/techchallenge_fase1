using System.Text.Json;

namespace FCG.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication UseUnauthorizedMiddleware(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == 401)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    Message = "Acesso negado. Faça login para continuar."
                }));
            }
        });
        return app;
    }
    
    public static WebApplication UseForbiddenMiddleware(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == 403)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    Message = "Acesso negado. Você não tem permissão para realizar esta ação."
                }));
            }
        });
        return app;
    }
}