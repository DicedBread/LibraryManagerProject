using System.Security.Claims;

namespace library_manager_server
{
    public class SessionHandler : ISessionHandler
    {
        public string ClaimName { get; } = "sessionId";


        private readonly Dictionary<string, long> _sessionsCache = new Dictionary<string, long>();
        private readonly ILogger<SessionHandler>? _logger;

        public SessionHandler() { }
        public SessionHandler(ILogger<SessionHandler> logger) => this._logger = logger;

        /// <summary>
        /// add a session to local cache
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="guid"></param>
        private void AddSession(long userId, Guid guid)
        {
            _logger?.LogInformation($"new session added userId:{userId} guid:{guid}");
            _sessionsCache.Add(guid.ToString(), userId);
        }


        public Guid CreateSession(long userId)
        {   
            Guid guid = Guid.NewGuid();
            AddSession(userId, guid);
            return guid;
        }

        /// <summary>
        /// remove session from local cache
        /// </summary>
        /// <param name="guid"></param>
        // <returns>true if session removed false if guid not found</returns>
        public bool KillSession(string guid)
        {
            _logger?.LogInformation($"removing session {guid}");
            return _sessionsCache.Remove(guid);
        }
        
        /// <summary>
        /// is given guid string a valid session key
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if id is a active session id</returns>
        public bool IsActiveSession(string id)
        {
            bool ret = _sessionsCache.ContainsKey(id.ToString());
            _logger?.LogInformation($"checking if guid: {id} is active session\t session count: {_sessionsCache.Count} \n\t res:{ret}");
            return ret;
        }


        public long? GetUserId(string providedSessionId)
        {
            if (_sessionsCache.TryGetValue(providedSessionId, out long userId))
            {
                return _sessionsCache[providedSessionId];
            }
            return null;
        }
        
        public string? GetSession(HttpContext httpContext)
        {
            Claim? sessionId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimName);
            return sessionId?.Value;
        }

    }
}
