using AutoMapper;
using Exceptions.Exeptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Task4.Controllers;
using Task4.DTOs;
using Task4.Mapper;
using Task4.Models;
using Task4.Repositories.Interfaces;
using Task4.Services;
using Task4.Services.Interfaces;
using Task4.Validator;

namespace Task4Test
{
    public class GetTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly MapperConfiguration _mapperConfig;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UsersController _controller;
        private readonly AbstractValidator<NewUserDTO> _validator;

        public GetTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });
            _mapper = _mapperConfig.CreateMapper();
            _userService = new UserService(_repositoryMock.Object, _mapper);
            _controller = new UsersController(_userService);
            _validator = new NewUserDTOValidator();
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult()
        {
            var expectedOutput = new List<User>
            {
                new User { Id = 1, FirstName = "Sima", LastName = "Simic", Telephone = "+381 11 123 45 67" },
                new User { Id = 2, FirstName = "Herbert", LastName = "Klein", Telephone = "+49 30 123 456 78 90" },
                new User { Id = 3, FirstName = "George", LastName = "Johanson", Telephone = "+1 800 123 4567" }
            };

            var mockOutput = expectedOutput.AsQueryable().BuildMock();

            _repositoryMock.Setup(x => x.GetAll())
                .Returns(mockOutput);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<User>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            //Check status code
            Assert.Equal(200, okResult.StatusCode);

            //Check value
            var jsonResult = Assert.IsType<List<User>>(okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult()
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
            var result = await _controller.GetById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            //Check status code
            Assert.Equal(200, okResult.StatusCode);

            //Check value
            var jsonResult = Assert.IsType<User>(okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound()
        {
            _repositoryMock.Setup(x => x.FindAsync(1))
                .Returns(Task.FromResult<User?>(null));

            // Act
            async Task Action() => await _controller.GetById(1);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Action);
        }
    }
}