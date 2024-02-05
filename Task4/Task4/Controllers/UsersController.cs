using Microsoft.AspNetCore.Mvc;
using Task4.DTOs;
using Task4.Models;
using Task4.Services.Interfaces;

namespace Task4.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>List of users.</returns>
        /// <response code="200">Returns the list of users</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return Ok(await _userService.GetAllUsers());
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <returns>Users info.</returns>
        /// <response code="200">Returns the users info</response>
        /// <response code="404">There is no user with given id.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetById([FromRoute] int id)
        {
            return Ok(await _userService.GetUserById(id));
        }

        /// <summary>
        /// Create new user. All fields are requested and telephone must be in format +(xxx) xxxxxxx.
        /// </summary>
        /// <returns>Message of successfull creating user.</returns>
        /// <response code="200">Successfully created user.</response>
        /// <response code="400">Bad Request because some of the request fields are invalid.</response>
        /// <response code="409">There is already user with given telephone.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Create([FromBody] NewUserDTO newUser)
        {
            await _userService.CreateNewUser(newUser);
            return Ok("Successfully created new user");
        }

        /// <summary>
        /// Delete existing user.
        /// </summary>
        /// <returns>Status code that there is no content</returns>
        /// <response code="204">Or successfully deleting user or user doesn't exit</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
