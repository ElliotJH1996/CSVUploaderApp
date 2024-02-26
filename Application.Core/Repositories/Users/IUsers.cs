
namespace Application.Core.Repositories.Users
{
    public interface IUsers
    {
        public int CheckUserCredentials(string username, string password);
    }
}
