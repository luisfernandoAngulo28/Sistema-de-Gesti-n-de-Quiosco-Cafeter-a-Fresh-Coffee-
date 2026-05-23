using QuioscoAPI.Models;

namespace QuioscoAPI.Repositories.Interfaces;

public interface IUserRepositoryLFAH
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(int id);
    Task<User?> GetByEmail(string email);
    Task<User> Create(User user);
    Task<User?> Update(int id, User user);
    Task<bool> Delete(int id);
}
