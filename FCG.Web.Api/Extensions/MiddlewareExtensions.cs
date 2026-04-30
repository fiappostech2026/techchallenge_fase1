using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FCG.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication UseErrorHandlingMiddleware(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                var tracingId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");

                var logger = context.RequestServices.GetRequiredService<ILogger<WebApplication>>();
                logger.LogError(ex, "Erro não tratado. TracingId: {TracingId}", tracingId);

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var resposta = new ErroResponse(
                        tracingId,
                        500,
                        "Ocorreu um erro interno no servidor.",
                        app.Environment.IsDevelopment() ? ex.Message : null
                    );

                    var opcoes = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(resposta, opcoes));
                }
            }
        });

        return app;
    }

    private sealed record ErroResponse(
        string TracingId,
        int Status,
        string Erro,
        string? Detalhe
    )
    {
        public string Timestamp { get; } = DateTime.UtcNow.ToString("o");
    }


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