namespace CreditManagementApi.Middlewares

{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found");

                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";

                var msg = new
                {
                    Message = ex.Message,
                    StatusCode = 404
                };

                await context.Response.WriteAsJsonAsync(msg);
            }

            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business rule violation");

                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";

                var msg = new
                {
                    Message = ex.Message,
                    StatusCode = 409
                };

                await context.Response.WriteAsJsonAsync(msg);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occured");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var msg = new
                {
                    Message = ex.Message,
                    statusCode = 500
                };
                await context.Response.WriteAsJsonAsync(msg);
            }
        }

    }
}
