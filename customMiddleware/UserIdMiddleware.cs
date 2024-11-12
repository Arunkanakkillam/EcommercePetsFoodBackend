using System.Security.Claims;
namespace EcommercePetsFoodBackend.customMiddleware
{
    
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserIdMiddleware> _logger;
        public UserIdMiddleware(RequestDelegate next, ILogger<UserIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                await _next(context);
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                _logger.LogInformation($"{context.User.Identity?.IsAuthenticated} checking true or false");

                var UserIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                _logger.LogInformation($"{UserIdClaim} userid");

                if (UserIdClaim != null)
                {
                    _logger.LogInformation($"id is {UserIdClaim.Value}");
                    context.Items["Id"] = UserIdClaim.Value;
                }
            }

            await _next(context);  
        }


    }
}
