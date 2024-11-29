using library_manager_server.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace library_manager_server
{
    public class SessionAuthoriizationHandler : AuthorizationHandler<ActiveSessionRequirement>
    {
        SessionHandler sessionHandler;
        public SessionAuthoriizationHandler(SessionHandler sessionHandler) => this.sessionHandler = sessionHandler;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveSessionRequirement requirement)
        {
            if(requirement == null) return Task.CompletedTask;

            if (context.User.HasClaim(c => c.Type == Account.SESSION_ID_NAME))
            {
                string providedSessionId = context.User.Claims.First(c => c.Type == Account.SESSION_ID_NAME).Value.ToString();
                if (sessionHandler.IsActiveSessionId(providedSessionId))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            };
            return Task.CompletedTask;
        }
    }
}
