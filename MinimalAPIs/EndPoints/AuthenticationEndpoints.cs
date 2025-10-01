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
        var group = routes.MapGroup("/api/Authentication").WithTags("Authentication");
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
                            ["email"] = new OpenApiString("universityuser@example.com"),
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
    }
}