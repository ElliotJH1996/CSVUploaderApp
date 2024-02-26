using Application.Core.BookServices;
using Application.Core.Models;
using Application.Core.Repositories;
using Application.Core.Repositories.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Services
{
    public class UServices
    {
        private readonly IUsers _user;
        private readonly ILogger<UServices> _logger;

        public UServices(IUsers u, ILogger<UServices> logger)
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
