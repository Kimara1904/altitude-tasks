using Microsoft.AspNetCore.Mvc;
using Task1.Services.Interfaces;

namespace Task1.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IService _service;

        public ImagesController(IService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all image names.
        /// </summary>
        /// <returns>List of image names.</returns>
        /// <response code="200">Returns the list of image numbers</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllNames()
        {
            return await _service.GetImagesNames();
        }

        /// <summary>
        /// Get image by its name.
        /// </summary>
        /// <returns>Image.</returns>
        /// <response code="200">Returns the image</response>
        /// <response code="404">There is no user with given name.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{name}")]
        public async Task<ActionResult> GetImage([FromRoute] string name)
        {
            return File(await _service.DownloadImage(name), "image/jpeg");
        }

        /// <summary>
        /// Add new image.
        /// </summary>
        /// <returns>Message of successfull adding image.</returns>
        /// <response code="200">Successfully added image.</response>
        /// <response code="400">Bad Request because file is not with good extension.</response>
        /// <response code="409">Image already exists.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<string>> AddImage(IFormFile newImage)
        {
            if (newImage == null || newImage.Length == 0)
            {
                return BadRequest("No image file provided");
            }

            var fileExtension = Path.GetExtension(newImage.FileName);
            if (string.IsNullOrEmpty(fileExtension) ||
                !fileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) &&
                !fileExtension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                !fileExtension.Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Provided file is not a valid image (only .jpg, .jpeg, and .png extensions are allowed)");
            }

            await _service.UploadImage(newImage);
            return Ok("Successfully uploaded image");
        }

        /// <summary>
        /// Delete image.
        /// </summary>
        /// <returns>Status code that there is no content</returns>
        /// <response code="204">Or successfully deleting image or image doesn't exit</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteImage([FromRoute] string name)
        {
            await _service.DeleteImage(name);
            return NoContent();
        }
    }
}
