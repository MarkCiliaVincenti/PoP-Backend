using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace PopLibrary
{
    /// <inheritdoc>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Users ValidUsers;

        public AuthenticationService(IOptions<Users> usersConfig)
        {
            ValidUsers = usersConfig.Value;
        }

        /// <inheritdoc>
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // TODO: Write code to get admin/standard user credentials fro config/db

            if (string.Equals(username, ValidUsers.AdminUser.Username, StringComparison.OrdinalIgnoreCase) 
                && string.Equals(password, ValidUsers.AdminUser.Password, StringComparison.Ordinal))
            {
                return new User { Id = username, Role = Role.Admin, Name = "Admin" };
            }
            else if (string.Equals(username, ValidUsers.GlobalUser.Username, StringComparison.OrdinalIgnoreCase)
                && string.Equals(password, ValidUsers.GlobalUser.Password, StringComparison.Ordinal))
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
