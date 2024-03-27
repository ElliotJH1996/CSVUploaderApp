using Application.Core.Repositories.Users;
using Microsoft.Extensions.Logging;

namespace Application.Core.Services
{
    public class UserServices
    {
        private readonly IUsers _user;
        private readonly ILogger<UserServices> _logger;

        public UserServices(IUsers u, ILogger<UserServices> logger)
        {
            _user = u;
            _logger = logger;
        }

        public int CheckUser(string username,string password) 
        {
            try
            {
                var userCheck = _user.CheckUserCredentials(username, password);
                return userCheck;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckUser");
                throw;
            }
        
        
        }
    }
}
