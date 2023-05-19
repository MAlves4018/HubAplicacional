using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WebApp.Services
{
    public class DynamicAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public DynamicAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var x = await base.GetPolicyAsync(policyName);
            return x
                   ?? new AuthorizationPolicyBuilder()
                       .AddRequirements(new DynamicAuthorizationRequirement(policyName))
                       .Build();
        }
    }
}