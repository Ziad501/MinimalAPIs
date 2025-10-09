using Domain.DTOs.AuthenticationDtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MinimalAPIs.Services;

public interface IAuthManager
{
    Task<Results<Ok<AuthenResDto>, BadRequest<List<ErrorResponseDto>>, UnauthorizedHttpResult>> Login(LoginDto dto);
    Task<Results<Created, BadRequest<List<ErrorResponseDto>>>> Register(RegisterDto dto);
}
