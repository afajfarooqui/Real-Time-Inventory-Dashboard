namespace Inventory_API.CustomMiddlewares
{
    public class RandomFailureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Random _random = new();
        public RandomFailureMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                if (_random.NextDouble() < 0.10) // 10% failure
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { error = "Simulated random failure" });
                    return;
                }
            }
            await _next(context);
        }
    }
}
