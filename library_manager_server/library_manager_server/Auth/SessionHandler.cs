namespace library_manager_server
{
    public class SessionHandler
    {
        Dictionary<String, double> sessionsCache = new Dictionary<String, double>();
        private readonly ILogger<SessionHandler>? logger;

        public SessionHandler() { }
        public SessionHandler(ILogger<SessionHandler> logger) => this.logger = logger;

        /// <summary>
        /// add a session to local cache
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="guid"></param>
        public void AddSession(double userId, Guid guid)
        {
            logger?.LogInformation($"new session added userId:{userId} guid:{guid}");
            sessionsCache.Add(guid.ToString(), userId);
        }

        /// <summary>
        /// remove session from local cache
        /// </summary>
        /// <param name="guid"></param>
        // <returns>true if session removed false if guid not found</returns>
        public bool RemoveSession(string guid)
        {
            logger?.LogInformation($"removing session {guid}");
            return sessionsCache.Remove(guid);
        }
        
        /// <summary>
        /// is given guid string a valid session key
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if id is a active session id</returns>
        public bool IsActiveSessionId(string id)
        {
            bool ret = sessionsCache.ContainsKey(id.ToString());
            logger?.LogInformation($"checking if guid: {id} is active session\t session count: {sessionsCache.Count} \n\t res:{ret}");
            return ret;
        }
    }
}
