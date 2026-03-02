using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Features.Auth;
using Eshop.Domain.Entities.People;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Configuration;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/login", async (
            [FromBody] LoginRequest request,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Results.Unauthorized();
            }

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return Results.Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                              ?? throw new InvalidOperationException("JwtSettings configuration is missing");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new AuthResponse
            {
                Success = true,
                Token = tokenString,
                User = new UserDto
                {
                    Id = Guid.Parse(user.Id.ToString()),
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                }
            };

            return Results.Ok(response);
        });
    }
}
