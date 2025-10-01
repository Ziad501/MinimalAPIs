using Domain.DTOs.AuthenticationDtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MinimalAPIs.Services;

public interface IAuthManager
{
    Task<Results<Ok<AuthenResDto>, UnauthorizedHttpResult>> Login(LoginDto dto);
}
