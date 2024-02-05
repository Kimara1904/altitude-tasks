using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task4.Controllers;
using Task4.Models;
using Task4.Repositories.Interfaces;
using Task4.Services;

namespace Task4Test
{
    public class DeleteTests
    {
        [Fact]
        public async Task DeleteUser_ReturnsOkResult()
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
            var result = await controller.Delete(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var NoContentResult = Assert.IsType<NoContentResult>(actionResult);

            //Check status code
            Assert.Equal(204, NoContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound()
        {
            var repositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var useCaseServices = new UserService(repositoryMock.Object, mapperMock.Object);
            var controller = new UsersController(useCaseServices);


            repositoryMock.Setup(x => x.FindAsync(1))
                .Returns(Task.FromResult<User?>(null));

            // Act
            var result = await controller.Delete(1);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var NoContentResult = Assert.IsType<NoContentResult>(actionResult);

            //Check status code
            Assert.Equal(204, NoContentResult.StatusCode);
        }
    }
}
