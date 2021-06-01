using System.Threading.Tasks;

namespace DAL.Security.Abstract
{
    public interface ISecurityService
    {
        Task<User> GetUser(int userId);
        Task<User[]> GetUsers();
        Task<bool> CreateUser(User user);
        Task<bool> EditUser(User user);
        Task<bool> DeleteUser(int userId);
    }
}