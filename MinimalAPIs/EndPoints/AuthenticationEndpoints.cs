using Domain.DTOs.AuthenticationDtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MinimalAPIs.Services;

namespace MinimalAPIs.EndPoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/authentication").WithTags("Authentication");

        group.MapPost("/login",
           async Task<Results<Ok<AuthenResDto>, UnauthorizedHttpResult>> (
               LoginDto dto,
               IAuthManager auth) =>
           {
               return await auth.Login(dto);
           })
        .WithName("Login")
        .Accepts<LoginDto>("application/json")
        .Produces<AuthenResDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithOpenApi(op =>
        {
            op.Summary = "Authenticate and obtain a JWT access token";
            op.Description =
                "Validates the supplied credentials using ASP.NET Core Identity. " +
                "On success, returns a signed JWT you can use in the `Authorization: Bearer <token>` header " +
                "for subsequent requests.";

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

            op.Responses[StatusCodes.Status200OK.ToString()]!.Content["application/json"]!.Example =
                new OpenApiObject
                {
                    ["userId"] = new OpenApiString("4f5a5b0d-7b3a-4e4e-8e8d-1234567890ab"),
                    ["token"] = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")
                };

            return op;
        });

        group.MapPost("/register",
           async Task<Results<Created, BadRequest<List<ErrorResponseDto>>>> (
               RegisterDto dto,
               IAuthManager auth) =>
           {
               return await auth.Register(dto);
           })
        .WithName("Register")
        .Accepts<RegisterDto>("application/json")
        .Produces(StatusCodes.Status201Created)
        .Produces<List<ErrorResponseDto>>(StatusCodes.Status400BadRequest)
        .WithOpenApi(op =>
        {
            op.Summary = "Register a new user account";
            op.Description =
                "Creates a new user using ASP.NET Core Identity. " +
                "On success, returns 201 Created with a Location header pointing to the new user's resource.";

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
            op.Responses[StatusCodes.Status201Created.ToString()] = new OpenApiResponse
            {
                Description = "User created.",
                Headers = new Dictionary<string, OpenApiHeader>
                {
                    ["Location"] = new OpenApiHeader
                    {
                        Description = "URI of the newly created user resource.",
                        Schema = new OpenApiSchema { Type = "string", Format = "uri" },
                        Example = new OpenApiString("/api/users/4f5a5b0d-7b3a-4e4e-8e8d-1234567890ab")
                    }
                }
            };
            op.Responses[StatusCodes.Status400BadRequest.ToString()] = new OpenApiResponse
            {
                Description = "Validation or identity errors.",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "array",
                            Items = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["code"]        = new OpenApiSchema { Type = "string" },
                                    ["description"] = new OpenApiSchema { Type = "string" }
                                }
                            }
                        },
                        Example = new OpenApiArray
                        {
                            new OpenApiObject
                            {
                                ["code"]        = new OpenApiString("DuplicateUserName"),
                                ["description"] = new OpenApiString("Email 'amira@example.com' is already taken.")
                            }
                        }
                    }
                }
            };

            return op;
        });
    }
}
