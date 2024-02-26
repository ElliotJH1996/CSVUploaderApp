using Application.Core.Models;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Repositories.Users
{
    public class Users : IUsers
    {
        private readonly ILogger<BookRepository> _log;
        private readonly IDbConnection _db;
        public Users(IDbConnection db, ILogger<BookRepository> log)
        {
            _db = db;
            _log = log;
        }

        public int CheckUserCredentials(string username, string password) 
        {
            try
            {
                var storedProcedureName = "sp_GetUser";

                var result = _db.Query<string>(storedProcedureName, new { username, password }, commandType: CommandType.StoredProcedure).Count();

                return result;

            }
            catch (Exception e)
            {
                _log.LogError(e,"CheckUserCredentials");
                throw;
            }


        }
    }
}
