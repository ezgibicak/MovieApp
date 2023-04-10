using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesAPI.Filters
{
    public class CustomActionFilter : IActionFilter
    {
        private readonly ILogger<CustomActionFilter> logger;

        public CustomActionFilter(ILogger<CustomActionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogWarning("Action executed");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogWarning("Action executing");
        }
    }
}
