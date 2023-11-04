using ICAP_FriendService.Entities;
using ICAP_FriendService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICAP_FriendService.Controllers
{
    [ApiController]
    [Route("friendrequests")]
    public class FriendRequestsController : ControllerBase
    {
        private readonly FriendRequestsRepository _friendRequestsRepository = new();

        [HttpGet]
        public async Task<IEnumerable<FriendRequest>> GetAsync()
        {
            var items = await _friendRequestsRepository.GetAllAsync();
            return items;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<FriendRequest>> GetByIdAsync(Guid id)
        {
            var item = await _friendRequestsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<FriendRequest>> SendAsync(FriendRequest data)
        {
            var request = new FriendRequest
            {
                UserFrom = data.UserFrom,
                UserTo = data.UserTo,
                Accepted = false,
                Declined = false,
                Pending = true,
                CreatedDateTime = DateTimeOffset.Now
            };

            await _friendRequestsRepository.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AcceptAsync(Guid id)
        {
            var existingItem = await _friendRequestsRepository.GetAsync(id);
            if (existingItem == null) return NotFound();
            existingItem.Accepted = true;
            existingItem.Declined = false;
            existingItem.Pending = false;
            
            await _friendRequestsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeclineAsync(Guid id)
        {
            var existingItem = await _friendRequestsRepository.GetAsync(id);
            if (existingItem == null) return NotFound();
            existingItem.Accepted = false;
            existingItem.Declined = true;
            existingItem.Pending = false;

            await _friendRequestsRepository.UpdateAsync(existingItem);

            return NoContent();
        }
    }
}
