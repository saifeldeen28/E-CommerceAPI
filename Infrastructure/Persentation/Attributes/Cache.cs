using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persentation.Attributes
{
    public class Cache:ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string CacheKey = GenerateCacheKey(context.HttpContext.Request);
            ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheValue = await cacheService.GetAsync(CacheKey);
            if (cacheValue != null) 
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
           var ExecutedContext =await next.Invoke();
            if (ExecutedContext.Result is OkObjectResult objectResult )
            {
                await cacheService.SetAsync(CacheKey, objectResult.Value, TimeSpan.FromMinutes(5));
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append($"{request.Path}?");
            foreach (var (keyName, value) in request.Query.OrderBy(x => x.Key))
            {
                key.Append($"{keyName}={value}&");
            }
            return key.ToString();

        }
    }
}
