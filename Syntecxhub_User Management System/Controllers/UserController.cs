using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Syntecxhub_User_Management_System.Models;
using Syntecxhub_User_Management_System.Repositories;

namespace Syntecxhub_User_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository; 
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            await _userRepository.Create(user);
            // This returns the full object + a 'Location' header pointing to your Get method
            return CreatedAtAction(nameof(Get), new { id = user.Id.ToString() }, user);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            // Validates that the string is a 24-character hex to prevent a 500 crash
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format. Must be a 24-digit hex string.");
            }

            var user = await _userRepository.Get(objectId);

            // If MongoDB returns null, send 404 instead of an empty 200
            if (user == null)
            {
                return NotFound($"User with ID {id} was not found.");
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, User user)
        {
            // Fix: Remove spaces and validate 24-char hex format
            if (!ObjectId.TryParse(id.Trim(), out var objectId))
            {
                return BadRequest("Invalid ID format. Must be a 24-digit hex string.");
            }

            // Ensure the ID in the URL is assigned to the user object
            user.Id = objectId;

            await _userRepository.Update(objectId, user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            await _userRepository.Delete(objectId);
            return Ok("User Deleted Successfully");
        }
        [HttpGet("Fetch")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAll();
            return Ok(users);
        }
    }
}
