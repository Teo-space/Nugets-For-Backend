using Serilog.Formatting.Compact;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;

namespace Api.Logging;

public static class Formatters
{
    public static MessageTemplateTextFormatter MessageTemplate { get; } = new(Templates.Body);

    public static JsonFormatter Json { get; } = new();
    public static CompactJsonFormatter CompactJson { get; } = new();
    public static RenderedCompactJsonFormatter RenderedCompactJson { get; } = new();

}