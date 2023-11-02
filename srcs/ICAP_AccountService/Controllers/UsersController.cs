using ICAP_AccountService.Entities;
using ICAP_AccountService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICAP_AccountService.Controllers
{
    [ApiController]
    [Route("friendrequests")]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _usersRepository = new();

        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            var items = await _usersRepository.GetAllAsync();
            return items;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<User>> GetByIdAsync(string id)
        {
            var item = await _usersRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostAsync(User data)
        {
            var request = new User
            {
                Name = data.Name,
                Email = data.Email,
                FriendIds = new List<string>(),
                CreatedDateTime = DateTimeOffset.Now
            };

            await _usersRepository.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, User data)
        {
            var existingItem = await _usersRepository.GetAsync(id);
            if (existingItem == null) return NotFound();
            existingItem.Name = data.Name;
            existingItem.Email = data.Email;
            existingItem.FriendIds = data.FriendIds;
            
            await _usersRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var existingItem = await _usersRepository.GetAsync(id);
            if (existingItem == null) return NotFound();
            await _usersRepository.RemoveAsync(existingItem.Id);

            return NoContent();
        }
    }
}
