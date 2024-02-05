using Task4.DTOs;
using Task4.Models;

namespace Task4.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task CreateNewUser(NewUserDTO newUser);
        Task DeleteUser(int id);
    }
}
