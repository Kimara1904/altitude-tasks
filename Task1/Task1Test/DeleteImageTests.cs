using Microsoft.AspNetCore.Mvc;
using Moq;
using Task1.CacheSystem.Interfaces;
using Task1.Controllers;
using Task1.FileSystem.Interfaces;
using Task1.Services;
using Task1.Services.Interfaces;

namespace Task1Test
{
    public class DeleteImageTests
    {
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly Mock<ICacheSystem> _cacheMock;
        private readonly IService _service;
        private readonly ImagesController _controller;

        public DeleteImageTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _cacheMock = new Mock<ICacheSystem>();
            _service = new Service(_cacheMock.Object, _fileSystemMock.Object);
            _controller = new ImagesController(_service);
        }

        [Fact]
        public async void DeleteImage_ThereIsImage_NoContent()
        {
            //Arrange
            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync("hash");

            //Act
            var result = await _controller.DeleteImage("hash");

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var NoContentResult = Assert.IsType<NoContentResult>(actionResult);

            //Check status code
            Assert.Equal(204, NoContentResult.StatusCode);
        }

        [Fact]
        public async void DeleteImage_ThereIsNoImage_NoContent()
        {
            //Arrange
            _fileSystemMock.Setup(fs => fs.ImageExist(It.IsAny<string>()))
                .Returns(false);

            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            //Act
            var result = await _controller.DeleteImage("hash");

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            var NoContentResult = Assert.IsType<NoContentResult>(actionResult);

            //Check status code
            Assert.Equal(204, NoContentResult.StatusCode);
        }
    }
}
