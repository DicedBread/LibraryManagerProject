namespace library_manager_server;

public interface ISessionHandler
{
    public string ClaimName { get; }
    
    /// <summary>
    /// add a session to local cache
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="guid"></param>
    // void AddSession(double userId, Guid guid);
    
    /// <summary>
    /// create session for user
    /// </summary>
    /// <param name="userId">user a session is created for</param>
    /// <returns>session id associated with user</returns>
    Guid CreateSession(double userId);

    /// <summary>
    /// remove session from local cache
    /// </summary>
    /// <param name="guid"></param>
    // <returns>true if session removed false if guid not found</returns>
    bool KillSession(string guid);

    /// <summary>
    /// is given guid string a valid session key
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true if id is a active session id</returns>
    bool IsActiveSession(string id);

    string? GetSession(HttpContext httpContext);
    
    double? GetUserId(string providedSessionId);
}