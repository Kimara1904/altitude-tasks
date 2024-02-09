using Microsoft.AspNetCore.Mvc;
using Moq;
using Task1.CacheSystem.Interfaces;
using Task1.Controllers;
using Task1.FileSystem.Interfaces;
using Task1.Services;
using Task1.Services.Interfaces;

namespace Task1Test
{
    public class GetAllNamesTests
    {
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly Mock<ICacheSystem> _cacheMock;
        private readonly IService _service;
        private readonly ImagesController _controller;

        public GetAllNamesTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _cacheMock = new Mock<ICacheSystem>();
            _service = new Service(_cacheMock.Object, _fileSystemMock.Object);
            _controller = new ImagesController(_service);
        }

        [Fact]
        public async Task GetAllNames_OkResult()
        {
            //Arrange
            var expectedResult = new List<string?> { "hash1", "hash2", "hash3" };
            _fileSystemMock.Setup(fs => fs.GetFiles(It.IsAny<string>()))
            .Returns(expectedResult);

            _cacheMock.Setup(c => c.GetValue(It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            //Act
            var result = await _controller.GetAllNames();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            //Check status code
            Assert.Equal(200, okResult.StatusCode);

            //Check value
            var returnedList = Assert.IsType<List<string?>>(okResult.Value);
            Assert.Equal(expectedResult, returnedList);
        }
    }
}
