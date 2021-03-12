namespace Dcf.Wwp.Api.Library.Helpers
{
    public class DatabaseConfiguration
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public DatabaseConfiguration(string server, string database, string userId, string password)
        {
            Server = server;
            Database = database;
            UserId = userId;
            Password = password;
        }
    }
}