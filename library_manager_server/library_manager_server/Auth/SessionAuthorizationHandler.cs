using library_manager_server.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace library_manager_server
{
    public class SessionAuthorizationHandler(ISessionHandler sessionHandler)
        : AuthorizationHandler<ActiveSessionRequirement>
    {
        private readonly ISessionHandler _sessionHandler = sessionHandler;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveSessionRequirement requirement)
        {
            if(requirement == null) return Task.CompletedTask;

            if (context.User.HasClaim(c => c.Type == _sessionHandler.ClaimName))
            {
                string providedSessionId = context.User.Claims.First(c => c.Type == _sessionHandler.ClaimName).Value.ToString();
                if (_sessionHandler.IsActiveSession(providedSessionId))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            };
            return Task.CompletedTask;
        }
    }
}
