using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task4.Controllers;
using Task4.Mapper;
using Task4.Models;
using Task4.Repositories.Interfaces;
using Task4.Services;
using Task4.Services.Interfaces;

namespace Task4Test
{
    public class DeleteTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly MapperConfiguration _mapperConfig;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UsersController _controller;

        public DeleteTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });
            _mapper = _mapperConfig.CreateMapper();
            _userService = new UserService(_repositoryMock.Object, _mapper);
            _controller = new UsersController(_userService);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOkResult()
        {
            var expectedOutput = new User
            {
                Id = 1,
                FirstName = "Sima",
                LastName = "Simic",
                Telephone = "+381 11 123 45 67"
            };

            _repositoryMock.Setup(x => x.FindAsync(1))
                .Returns(Task.FromResult(expectedOutput)!);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var NoContentResult = Assert.IsType<NoContentResult>(actionResult);

            //Check status code
            Assert.Equal(204, NoContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound()
        {
            _repositoryMock.Setup(x => x.FindAsync(1))
                .Returns(Task.FromResult<User?>(null));

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var NoContentResult = Assert.IsType<NoContentResult>(actionResult);

            //Check status code
            Assert.Equal(204, NoContentResult.StatusCode);
        }
    }
}
