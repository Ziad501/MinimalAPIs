using Domain.DTOs.AuthenticationDtos;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIs.OpenApiSpecs;
using MinimalAPIs.Services;

namespace MinimalAPIs.EndPoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/authentication")
                          .WithTags("Authentication")
                          .WithGroupName("auth");

        group.MapPost("/login",
           async Task<Results<Ok<AuthenResDto>, BadRequest<List<ErrorResponseDto>>, UnauthorizedHttpResult>> (
               IValidator<LoginDto> validator,
               LoginDto dto,
               IAuthManager auth, CancellationToken ct) =>
           {
               var validationResult = await validator.ValidateAsync(dto, ct);
               if (!validationResult.IsValid)
               {
                   var errors = validationResult.Errors
                   .Select(e => new ErrorResponseDto(
                       string.IsNullOrEmpty(e.ErrorCode) ? e.PropertyName : e.ErrorCode,
                       e.ErrorMessage)).ToList();

                   return TypedResults.BadRequest(errors);
               }
               return await auth.Login(dto);
           })
        .WithName("Login")
        .Accepts<LoginDto>("application/json")
        .Produces<AuthenResDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithOpenApi(AuthSpecs.Login);

        group.MapPost("/register",
           async Task<Results<Created, BadRequest<List<ErrorResponseDto>>>> (
               IValidator<RegisterDto> validator,
               RegisterDto dto,
               IAuthManager auth,
               CancellationToken ct) =>
           {
               var validationResult = await validator.ValidateAsync(dto, ct);
               if (!validationResult.IsValid)
               {
                   var errors = validationResult.Errors
                   .Select(e => new ErrorResponseDto(
                       string.IsNullOrEmpty(e.ErrorCode) ? e.PropertyName : e.ErrorCode,
                       e.ErrorMessage)).ToList();

                   return TypedResults.BadRequest(errors);
               }
               return await auth.Register(dto);
           })
        .WithName("Register")
        .Accepts<RegisterDto>("application/json")
        .Produces(StatusCodes.Status201Created)
        .Produces<List<ErrorResponseDto>>(StatusCodes.Status400BadRequest)
        .WithOpenApi(AuthSpecs.Register);
    }
}
