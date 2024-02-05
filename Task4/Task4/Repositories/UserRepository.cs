using Microsoft.EntityFrameworkCore;
using Task4.Models;
using Task4.Repositories.Interfaces;

namespace Task4.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public void Delete(User entity)
        {
            _context.Set<User>().Remove(entity);
        }

        public async Task<User?> FindAsync(int id)
        {
            return await _context.Set<User>().FindAsync(id);
        }

        public IQueryable<User> GetAll()
        {
            return _context.Set<User>();
        }

        public async Task Insert(User entity)
        {
            await _context.Set<User>().AddAsync(entity);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
