using Microsoft.AspNetCore.Authorization;

namespace library_manager_server
{
    public class ActiveSessionRequirement : IAuthorizationRequirement
    {
        LibrayManager librayManager;

        public ActiveSessionRequirement(LibrayManager librayManager) => this.librayManager = librayManager;

        public bool IsActiveSessionId(string id)
        {
            Console.WriteLine(id);
            return librayManager.IsActiveSessionId(id);
        }
    }
}
