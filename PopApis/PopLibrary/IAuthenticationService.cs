using System.Threading.Tasks;

namespace PopLibrary
{
    /// <summary>
    /// Authentication service.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates users credentials.
        /// </summary>
        /// <param name="username">User id.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        Task<User> AuthenticateAsync(string username, string password);
    }
}
