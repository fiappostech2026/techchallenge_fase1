using Serilog;
using Serilog.Formatting.Compact;

namespace FCG.Extensions;

public static class SerilogExtension
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                formatter: new CompactJsonFormatter(),
                path: "logs/fcg-.json",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}
