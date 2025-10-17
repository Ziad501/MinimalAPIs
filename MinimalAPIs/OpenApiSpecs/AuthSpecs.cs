using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinimalAPIs.OpenApiSpecs
{
    public static class AuthSpecs
    {
        public static OpenApiOperation Login(OpenApiOperation op)
        {
            op.Summary = "Authenticate and obtain a JWT access token";
            op.Description = "Validates credentials and returns a signed JWT to use in Authorization: Bearer <token>.";
            op.RequestBody = new OpenApiRequestBody
            {
                Required = true,
                Content =
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Example = new OpenApiObject
                    {
                        ["email"]    = new OpenApiString("universityuser@example.com"),
                        ["password"] = new OpenApiString("User123!")
                    }
                }
            }
            };
            if (op.Responses.TryGetValue(StatusCodes.Status200OK.ToString(), out var ok) &&
                ok.Content.TryGetValue("application/json", out var media))
            {
                media.Example = new OpenApiObject
                {
                    ["userId"] = new OpenApiString("4f5a5b0d-7b3a-4e4e-8e8d-1234567890ab"),
                    ["token"] = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")
                };
            }
            return op;
        }

        public static OpenApiOperation Register(OpenApiOperation op)
        {
            op.Summary = "Register a new user account";
            op.Description = "Creates a new user with role 'User'. Returns 201 with Location header.";
            op.RequestBody = new OpenApiRequestBody
            {
                Required = true,
                Content =
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Example = new OpenApiObject
                    {
                        ["firstName"]   = new OpenApiString("Amira"),
                        ["lastName"]    = new OpenApiString("Ibrahim"),
                        ["email"]       = new OpenApiString("amira@example.com"),
                        ["password"]    = new OpenApiString("Str0ng!Passw0rd!"),
                        ["dateOfBirth"] = new OpenApiString("1998-04-12")
                    }
                }
            }
            };
            return op;
        }
    }
}