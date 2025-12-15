using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace MyFirstApp.CustomMiddlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string? email = string.Empty, password = string.Empty;
        StringBuilder errors = new StringBuilder();
        using StreamReader reader = new StreamReader(context.Request.Body);
        string requestBody = await reader.ReadToEndAsync();
        Dictionary<string, StringValues> queries = QueryHelpers.ParseQuery(requestBody);

        if (context.Request.Method.Equals(HttpMethod.Post.Method))
        {
            email = ValidateQuery("email", errors, queries);
            password = ValidateQuery("password", errors, queries);

            if (errors.Length > 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(errors.ToString());
                return;
            }
            if (email!.Equals("admin@example.com") && password!.Equals("admin1234"))
            {
                await context.Response.WriteAsync("login success");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Login");
            }
        }
        await _next(context);
    }

    private static string? ValidateQuery(string? input, StringBuilder errors, Dictionary<string, StringValues> queries)
    {
        if (string.IsNullOrWhiteSpace(input) || !queries.TryGetValue(input, out StringValues value))
        {
            errors.AppendLine($"invalid input for '{input}'");
            return default;
        }
        return value;
    }
}

public static class AuthenticationMiddlewareExtension
{
    public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthenticationMiddleware>();
    }
}
