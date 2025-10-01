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

    private async Task<string> GenerateTokenAsync(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        var userClaims = await userManager.GetClaimsAsync(user);

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("userId",user.Id),
        }.Union(userClaims).Union(roleClaims).ToList();

        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_config["JWT:DurationInHours"])),
            signingCredentials: credentials
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}