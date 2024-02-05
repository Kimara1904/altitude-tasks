using Task4.Models;

namespace Task4.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();
        Task<User?> FindAsync(int id);
        Task Insert(User entity);
        void Delete(User entity);
        Task Save();
    }
}
