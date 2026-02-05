using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace OperationManagementService.Security
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration configuration;

        public const string SchemaName = "AuthenticationKey";

        public CustomAuthenticationHandler(IConfiguration config, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            configuration = config;
        }
        // custom authentication handler to validate the AuthenticationKey header
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("AuthenticationKey"))
            {
                return AuthenticateResult.Fail("Missing Authentication");
            }
            if (!Guid.TryParse(Request.Headers["AuthenticationKey"], out Guid apiKey))
            {
                return AuthenticateResult.Fail("Invalid Authentication");
            }
            var AuthenticationKey = Guid.Parse(configuration["Authentication-Key"] ?? Guid.Empty.ToString());
            if (apiKey != AuthenticationKey)
            {
                return AuthenticateResult.Fail("Invalid Authentication");
            }

            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] {new(ClaimTypes.Authentication, apiKey.ToString())}, SchemaName)), SchemaName)));
        }
    }
}
