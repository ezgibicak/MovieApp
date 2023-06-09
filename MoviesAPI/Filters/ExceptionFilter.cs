﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesAPI.Filters
{
    public class ExceptionFilter:ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            //context.HttpContext
            logger.LogError(context.Exception,context.Exception.Message);
            base.OnException(context);
        }
    }
}
