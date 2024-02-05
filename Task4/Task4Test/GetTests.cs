using AutoMapper;
using Exceptions.Exeptions;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using Task4.Controllers;
using Task4.Models;
using Task4.Repositories.Interfaces;
using Task4.Services;

namespace Task4Test
{
    public class GetTests
    {
        [Fact]
        public async Task GetUsers_ReturnsOkResult()
        {
            var repositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var useCaseServices = new UserService(repositoryMock.Object, mapperMock.Object);
            var controller = new UsersController(useCaseServices);

            var expectedOutput = new List<User>
            {
                new User { Id = 1, FirstName = "Sima", LastName = "Simic", Telephone = "+381 11 123 45 67" },
                new User { Id = 2, FirstName = "Herbert", LastName = "Klein", Telephone = "+49 30 123 456 78 90" },
                new User { Id = 3, FirstName = "George", LastName = "Johanson", Telephone = "+1 800 123 4567" }
            };

            var mockOutput = expectedOutput.AsQueryable().BuildMock();

            repositoryMock.Setup(x => x.GetAll())
                .Returns(mockOutput);

            // Act
            var result = await controller.GetAll();

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
            var repositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var useCaseServices = new UserService(repositoryMock.Object, mapperMock.Object);
            var controller = new UsersController(useCaseServices);

            var expectedOutput = new User
            {
                Id = 1,
                FirstName = "Sima",
                LastName = "Simic",
                Telephone = "+381 11 123 45 67"
            };

            repositoryMock.Setup(x => x.FindAsync(1))
                .Returns(Task.FromResult(expectedOutput)!);

            // Act
            var result = await controller.GetById(1);

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
            var repositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var useCaseServices = new UserService(repositoryMock.Object, mapperMock.Object);
            var controller = new UsersController(useCaseServices);


            repositoryMock.Setup(x => x.FindAsync(1))
                .Returns(Task.FromResult<User?>(null));

            // Act
            async Task Action() => await controller.GetById(1);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Action);
        }
    }
}