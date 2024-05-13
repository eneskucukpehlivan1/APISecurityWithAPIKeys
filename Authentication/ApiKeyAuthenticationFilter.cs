using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APISecurityWithAPIKeys.Authentication
{
    public class ApiKeyAuthenticationFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var providedApiKey = context.HttpContext.Request.Headers[AuthConfig.ApiKeyHeader].FirstOrDefault();
            var isValid = IsValidApiKey(providedApiKey);

            if (!isValid)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Authentication");
                return;
            }
        }

        private bool IsValidApiKey(string providedApiKey)
        {
            if (String.IsNullOrEmpty(providedApiKey))
                return false;

            var validApiKey = _configuration.GetValue<string>(AuthConfig.AuthSection);

            return string.Equals(validApiKey, providedApiKey, StringComparison.Ordinal);
        }
    }
}
