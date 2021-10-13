using System;
using System.Threading.Tasks;

namespace PopLibrary
{
    /// <inheritdoc>
    public class AuthenticationService : IAuthenticationService
    {
        private const string AdminUserId = "popadmin";
        private const string AdminPassword = "popadminpassword";

        private const string StandardUserId = "popstandarduser";
        private const string StandardUserPassword = "popstandarduserpassword";

        /// <inheritdoc>
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // TODO: Write code to get admin/standard user credentials fro config/db

            if (string.Equals(username, AdminUserId, StringComparison.OrdinalIgnoreCase) 
                && string.Equals(password, AdminPassword, StringComparison.Ordinal))
            {
                return new User { Id = username, Role = Role.Admin, Name = "Admin" };
            }
            else if (string.Equals(username, StandardUserId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(password, StandardUserPassword, StringComparison.Ordinal))
            {
                return new User { Id = username, Role = Role.User, Name = "User" };
            }
            else
            {
                return null;
            }
        }
    }
}
