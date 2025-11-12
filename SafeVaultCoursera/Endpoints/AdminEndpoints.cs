namespace SafeVaultCoursera.Endpoints
{
    public static class AdminEndpoints
    {
        public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/admin");

            group.MapGet("/dashboard", () =>
            {
                return Results.Ok(new { Message = "Bem-vindo ao Dashboard do Admin!" });
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

            group.MapGet("/profile", (System.Security.Claims.ClaimsPrincipal user) =>
            {
                var username = user.Identity?.Name;
                return Results.Ok(new { Message = $"Bem-vindo, {username}!" });
            })
            .RequireAuthorization();
        }
    }
}
