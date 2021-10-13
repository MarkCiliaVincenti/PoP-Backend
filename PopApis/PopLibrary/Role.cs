namespace PopLibrary
{
    /// <summary>
    /// Record that defines the roles in PoP service.
    /// </summary>
    public record Role
    {
        /// <summary>
        /// Admin user.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Standard (non-admin) user.
        /// </summary>
        public const string User = "User";
    }
}
