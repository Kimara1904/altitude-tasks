using Exceptions.Exeptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task1.CacheSystem.Interfaces;
using Task1.Controllers;
using Task1.FileSystem.Interfaces;
using Task1.Services;
using Task1.Services.Interfaces;

namespace Task1Test
{
    public class DownloadImageTests
    {
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly Mock<ICacheSystem> _cacheMock;
        private readonly IService _service;
        private readonly ImagesController _controller;

        public DownloadImageTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _cacheMock = new Mock<ICacheSystem>();
            _service = new Service(_cacheMock.Object, _fileSystemMock.Object);
            _controller = new ImagesController(_service);
        }

        [Fact]
        public async Task DownloadImage_ThereIsInCash_OkResult()
        {
            //Arrange
            var expectedResult = new byte[] { 0x00, 0x01, 0x02 };

            _fileSystemMock.Setup(fs => fs.GetImage(It.IsAny<string>()))
                .ReturnsAsync((expectedResult));

            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync("hash");

            // Act
            var result = await _controller.GetImage("hash");

            // Assert
            var okResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("image/jpeg", okResult.ContentType);
            Assert.Equal(expectedResult, okResult.FileContents);
        }

        [Fact]
        public async Task DownloadImage_ThereIsInFolder_OkResult()
        {
            //Arrange
            var expectedResult = new byte[] { 0x00, 0x01, 0x02 };

            _fileSystemMock.Setup(fs => fs.GetImage(It.IsAny<string>()))
                .ReturnsAsync((expectedResult));

            _fileSystemMock.Setup(fs => fs.ImageExist(It.IsAny<string>()))
                .Returns(true);

            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            // Act
            var result = await _controller.GetImage("hash");

            // Assert
            var okResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("image/jpeg", okResult.ContentType);
            Assert.Equal(expectedResult, okResult.FileContents);
        }

        [Fact]
        public async Task AddImage_ThereIsNoImage_ReturnsNoFound()
        {
            //Arrange
            _fileSystemMock.Setup(fs => fs.ImageExist(It.IsAny<string>()))
                .Returns(false);

            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            //Act
            async Task Action() => await _controller.GetImage("hash");

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Action);
        }
    }
}
