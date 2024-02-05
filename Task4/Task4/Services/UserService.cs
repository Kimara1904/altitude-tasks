using AutoMapper;
using Exceptions.Exeptions;
using Microsoft.EntityFrameworkCore;
using Task4.DTOs;
using Task4.Models;
using Task4.Repositories.Interfaces;
using Task4.Services.Interfaces;

namespace Task4.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateNewUser(NewUserDTO newUser)
        {
            var existingUser = await _userRepository.GetAll().Where(u => u.Telephone.Equals(newUser.Telephone)).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                throw new ConflictException($"There is already user with telephone: {newUser.Telephone}");
            }

            var user = _mapper.Map<User>(newUser);

            await _userRepository.Insert(user);
            await _userRepository.Save();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.FindAsync(id);

            if (user == null)
            {
                return;
            }

            _userRepository.Delete(user);
            await _userRepository.Save();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAll().ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _userRepository.FindAsync(id)
                ?? throw new NotFoundException($"There is no user with id: {id}");

            return user;
        }
    }
}
