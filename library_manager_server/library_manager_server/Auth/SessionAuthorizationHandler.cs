using System.Security.Claims;
using library_manager_server.Controllers;
using library_manager_server.ClientContext;
using Microsoft.AspNetCore.Authorization;

namespace library_manager_server
{
    public class SessionAuthorizationHandler(ISessionHandler sessionHandler, ILogger<SessionAuthorizationHandler> logger)
        : AuthorizationHandler<ActiveSessionRequirement>
    {
        private readonly ISessionHandler _sessionHandler = sessionHandler;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveSessionRequirement requirement)
        {
            string o = "";
            foreach (Claim claim in context.User.Claims)
            {
                o += $"Claim type: {claim.Type} claim value: {claim.Value}\n";
            }
            logger.LogInformation("Handling session requirement\n " + o);

            
            if (context.User.HasClaim(c => c.Type == _sessionHandler.ClaimName))
            {
                string providedSessionId = context.User.Claims.First(c => c.Type == _sessionHandler.ClaimName).Value.ToString();
                if (_sessionHandler.IsActiveSession(providedSessionId))
                {
                    logger.LogInformation($"Session {providedSessionId} is active");
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            };
            logger.LogInformation($"Inactive session");
            return Task.CompletedTask;
        }
    }
}
