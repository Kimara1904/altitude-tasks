using AutoMapper;
using Exceptions.Exeptions;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Task4.Controllers;
using Task4.DTOs;
using Task4.Mapper;
using Task4.Models;
using Task4.Repositories.Interfaces;
using Task4.Services;
using Task4.Validator;

namespace Task4Test
{
    public class CreateTests
    {
        [Fact]
        public async Task CreateUser_ReturnsOkResult()
        {
            var repositoryMock = new Mock<IUserRepository>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            var useCaseServices = new UserService(repositoryMock.Object, mapper);
            var controller = new UsersController(useCaseServices);

            var expectedOutput = new List<User>
            {
                new User { Id = 1, FirstName = "Sima", LastName = "Simic", Telephone = "+381 11 123 45 67" },
                new User { Id = 2, FirstName = "Herbert", LastName = "Klein", Telephone = "+49 30 123 456 78 90" }
            };

            var mockOutput = expectedOutput.AsQueryable().BuildMock();

            repositoryMock.Setup(x => x.GetAll())
                .Returns(mockOutput);

            // Act
            var result = await controller.Create(new NewUserDTO
            {
                FirstName = "George",
                LastName = "Johanson",
                Telephone = "+1 800 123 4567"
            });

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            //Check status code
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateUserWithExistingTelephone_ReturnsNotFound()
        {
            var repositoryMock = new Mock<IUserRepository>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            var useCaseServices = new UserService(repositoryMock.Object, mapper);
            var controller = new UsersController(useCaseServices);

            var expectedOutput = new List<User>
            {
                new User { Id = 1, FirstName = "Sima", LastName = "Simic", Telephone = "+381 11 123 45 67" },
                new User { Id = 2, FirstName = "Herbert", LastName = "Klein", Telephone = "+49 30 123 456 78 90" }
            };

            var mockOutput = expectedOutput.AsQueryable().BuildMock();

            repositoryMock.Setup(x => x.GetAll())
                .Returns(mockOutput);

            // Act
            async Task Action()
            {
                await controller.Create(new NewUserDTO
                {
                    FirstName = "George",
                    LastName = "Johanson",
                    Telephone = "+49 30 123 456 78 90"
                });
            }

            // Assert
            await Assert.ThrowsAsync<ConflictException>(Action);
        }

        [Fact]
        public async Task CreateUser_EmptyFirstName()
        {
            // Arrange
            var validator = new NewUserDTOValidator();
            var request = new NewUserDTO
            {
                FirstName = "",
                LastName = "Johanson",
                Telephone = "+49 30 123 456 78 90"
            };

            // Act
            var validationResult = await validator.ValidateAsync(request);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task CreateUser_EmptyLarstName()
        {
            // Arrange
            var validator = new NewUserDTOValidator();
            var request = new NewUserDTO
            {
                FirstName = "George",
                LastName = "",
                Telephone = "+49 30 123 456 78 90"
            };

            // Act
            var validationResult = await validator.ValidateAsync(request);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task CreateUser_EmptyPhone()
        {
            // Arrange
            var validator = new NewUserDTOValidator();
            var request = new NewUserDTO
            {
                FirstName = "George",
                LastName = "Johanson",
                Telephone = ""
            };

            // Act
            var validationResult = await validator.ValidateAsync(request);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public async Task CreateUser_BadFormatPhone()
        {
            // Arrange
            var validator = new NewUserDTOValidator();
            var request = new NewUserDTO
            {
                FirstName = "George",
                LastName = "Johanson",
                Telephone = "grr3ed32r434323"
            };

            // Act
            var validationResult = await validator.ValidateAsync(request);

            // Assert
            Assert.False(validationResult.IsValid);
        }
    }
}
