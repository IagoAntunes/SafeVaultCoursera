using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeVaultCoursera.Models;
using SafeVaultCoursera.Services;

namespace SafeVaultCoursera.Endpoints
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/account");

            group.MapPost("/register", async (
                [FromBody] RegisterDto model,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager) =>
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
                }

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }

                await userManager.AddToRoleAsync(user, "User");

                return Results.Ok(new { Message = "Usuário registrado com sucesso." });
            });

            group.MapPost("/login", async (
                [FromBody] LoginDto model,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                TokenService tokenService) =>
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                {
                    return Results.Unauthorized();
                }

                // Gera o Token JWT
                var token = await tokenService.GenerateToken(user);
                return Results.Ok(new { token = token });
            });
        }
    }
}
