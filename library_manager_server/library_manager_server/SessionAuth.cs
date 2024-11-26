using library_manager_server.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace library_manager_server
{
    public class SessionAuth : AuthorizationHandler<ActiveSessionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveSessionRequirement requirement)
        {
            if(requirement == null) return Task.CompletedTask;

            if (context.User.HasClaim(c => c.Type == Authentication.SESSION_ID_NAME))
            {
                string providedSessionId = context.User.Claims.First(c => c.Type == Authentication.SESSION_ID_NAME).Value.ToString();
                if (requirement.IsActiveSessionId(providedSessionId))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            };
            return Task.CompletedTask;
        }
    }
}
