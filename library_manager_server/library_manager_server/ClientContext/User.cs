namespace library_manager_server.ClientContext
{
    public class User
    {
        public User(ServerContext.User user){
            Id = user.UserId;
            Email = user.Email;
            Username = user.Username;
        }

        public User(){}

        public long Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
