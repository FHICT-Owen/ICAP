using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ICAP_RelationService.Controllers
{
    [ApiController]
    [Route("friendrequests")]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IRepository<FriendRequest> _friendRequestsRepository;

        public FriendRequestsController(IRepository<FriendRequest> friendRequestsRepository)
        {
            _friendRequestsRepository = friendRequestsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<FriendRequest>> GetAsync()
        {
            var items = await _friendRequestsRepository.GetAllAsync();
            return items;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<FriendRequest>> GetByIdAsync(string id)
        {
            var item = await _friendRequestsRepository.GetAsync(id);
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
        public async Task<IActionResult> AcceptAsync(string id)
        {
            var existingItem = await _friendRequestsRepository.GetAsync(id);
            existingItem.Accepted = true;
            existingItem.Declined = false;
            existingItem.Pending = false;
            
            await _friendRequestsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeclineAsync(string id)
        {
            var existingItem = await _friendRequestsRepository.GetAsync(id);
            existingItem.Accepted = false;
            existingItem.Declined = true;
            existingItem.Pending = false;
            await _friendRequestsRepository.UpdateAsync(existingItem);
            return NoContent();
        }
    }
}
