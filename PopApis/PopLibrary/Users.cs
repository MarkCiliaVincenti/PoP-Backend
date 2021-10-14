namespace PopLibrary
{
    public class Users
    {
        public UserData GlobalUser { get; set; }

        public UserData AdminUser { get; set; }
    }

    public class UserData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
