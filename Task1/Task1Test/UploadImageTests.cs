using Exceptions.Exeptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task1.CacheSystem.Interfaces;
using Task1.Controllers;
using Task1.FileSystem.Interfaces;
using Task1.Services;
using Task1.Services.Interfaces;

namespace Task1Test
{
    public class UploadImageTests
    {
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly Mock<ICacheSystem> _cacheMock;
        private readonly Mock<IFormFile> _fileMock;
        private readonly IService _service;
        private readonly ImagesController _controller;

        public UploadImageTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _cacheMock = new Mock<ICacheSystem>();
            _service = new Service(_cacheMock.Object, _fileSystemMock.Object);
            _controller = new ImagesController(_service);
            _fileMock = new Mock<IFormFile>();
        }

        [Fact]
        public async Task AddImage_OkResult()
        {
            //Arrange
            _fileSystemMock.Setup(fs => fs.ImageExist(It.IsAny<string>()))
                .Returns(false);

            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            _fileMock.SetupGet(f => f.FileName)
                .Returns("image.png");
            _fileMock.SetupGet(f => f.Length)
                .Returns(100);
            //Act
            var result = await _controller.AddImage(_fileMock.Object);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            //Check status code
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task AddImage_ThereIsInCashe_ReturnsConflict()
        {
            //Arrange
            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync("hash");

            _fileMock.SetupGet(f => f.FileName)
                .Returns("image.png");
            _fileMock.SetupGet(f => f.Length)
                .Returns(100);

            //Act
            async Task Action() => await _controller.AddImage(_fileMock.Object);

            // Assert
            await Assert.ThrowsAsync<ConflictException>(Action);
        }

        [Fact]
        public async Task AddImage_ThereIsInFolder_ReturnsConflict()
        {
            //Arrange
            _fileSystemMock.Setup(fs => fs.ImageExist(It.IsAny<string>()))
                .Returns(true);

            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            _fileMock.SetupGet(f => f.FileName)
                .Returns("image.png");
            _fileMock.SetupGet(f => f.Length)
                .Returns(100);

            //Act
            async Task Action() => await _controller.AddImage(_fileMock.Object);

            // Assert
            await Assert.ThrowsAsync<ConflictException>(Action);
        }

        [Fact]
        public async Task AddImage_WrongExtension_ReturnsConflict()
        {
            //Arrange
            _fileMock.SetupGet(f => f.FileName)
                .Returns("file.txt");
            _fileMock.SetupGet(f => f.Length)
                .Returns(100);

            //Act
            var result = await _controller.AddImage(_fileMock.Object);

            // Assert
            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

            //Check status code
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}