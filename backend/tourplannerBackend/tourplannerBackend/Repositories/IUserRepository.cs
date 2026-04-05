using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<bool> ExistsAsync(string username);
    }
}
