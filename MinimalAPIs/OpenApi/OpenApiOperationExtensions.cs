using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

public static class OpenApiOperationExtensions
{
    /// Sets description for a path parameter (case-insensitive). No-throw if missing.
    public static OpenApiOperation SetPathParamDescription(
        this OpenApiOperation op, string paramName, string description)
    {
        var p = op.Parameters?.FirstOrDefault(
            x => string.Equals(x.Name, paramName, StringComparison.OrdinalIgnoreCase));
        if (p is not null) p.Description = description;
        return op;
    }

    /// Convenience for "id".
    public static OpenApiOperation SetIdDescription(this OpenApiOperation op, string description)
        => op.SetPathParamDescription("id", description);

    /// Adds/overwrites a JSON example for a given status/content type.
    public static OpenApiOperation SetJsonExample(
        this OpenApiOperation op, string statusCode, IOpenApiAny example, string contentType = "application/json")
    {
        if (op.Responses.TryGetValue(statusCode, out var resp) &&
            resp.Content.TryGetValue(contentType, out var media))
        {
            media.Example = example;
        }
        return op;
    }

    /// Adds/overwrites a response header schema on a status code.
    public static OpenApiOperation EnsureResponseHeader(
        this OpenApiOperation op, string statusCode, string headerName, string description, string example)
    {
        if (!op.Responses.TryGetValue(statusCode, out var resp))
            return op;

        resp.Headers ??= new Dictionary<string, OpenApiHeader>(StringComparer.OrdinalIgnoreCase);
        resp.Headers[headerName] = new OpenApiHeader
        {
            Description = description,
            Schema = new OpenApiSchema { Type = "string", Example = new OpenApiString(example) }
        };
        return op;
    }
}