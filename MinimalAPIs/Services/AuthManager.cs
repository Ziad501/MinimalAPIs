using Domain.DTOs.AuthenticationDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalAPIs.Services;

public class AuthManager(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration _config) : IAuthManager
{
    public async Task<Results<Ok<AuthenResDto>, UnauthorizedHttpResult>> Login(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null) return TypedResults.Unauthorized();

        var isValidCredentials = await signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
        if (!isValidCredentials.Succeeded) return TypedResults.Unauthorized();

        var token = await GenerateTokenAsync(user);

        return TypedResults.Ok(new AuthenResDto(user.Id, token));
    }
    public async Task<Results<Created, BadRequest<List<ErrorResponseDto>>>> Register(RegisterDto dto)
    {
        User user = new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email,
            DateOfBirth = dto.DateOfBirth,
        };
        var createResult = await userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
        {
            return BadRequestFromErrors(createResult.Errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            return BadRequestFromErrors(roleResult.Errors);

        }
        return TypedResults.Created($"/api/users/{user.Id}");
    }

    private async Task<string> GenerateTokenAsync(User user)
    {
        var key = _config["JWT:Key"] ?? throw new InvalidOperationException("JWT:Key missing");
        var issuer = _config["JWT:Issuer"];
        var audience = _config["JWT:Audience"];
        if (!int.TryParse(_config["JWT:DurationInHours"], out var hours)) hours = 12;

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var roles = await userManager.GetRolesAsync(user);
        var userClaims = await userManager.GetClaimsAsync(user);

        var claims = new List<Claim>()
    {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new("userId", user.Id),
    };

        claims.AddRange(userClaims);
        foreach (var r in roles)
            claims.Add(new Claim(ClaimTypes.Role, r));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(hours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static BadRequest<List<ErrorResponseDto>> BadRequestFromErrors(IEnumerable<IdentityError> errors)
    {
        var list = new List<ErrorResponseDto>();
        foreach (var e in errors)
            list.Add(new ErrorResponseDto(e.Code, e.Description));

        return TypedResults.BadRequest(list);
    }
}