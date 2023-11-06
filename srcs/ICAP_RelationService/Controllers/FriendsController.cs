using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ICAP_RelationService.Controllers
{
    [ApiController]
    [Route("friends")]
    public class FriendsController : ControllerBase
    {
        private readonly IRepository<Friends> _friendsRepository;

        public FriendsController(IRepository<Friends> friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Friends>> GetAsync()
        {
            var items = await _friendsRepository.GetAllAsync();
            return items;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Friends>> GetByIdAsync(string id)
        {
            var item = await _friendsRepository.GetAsync(id);
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Friends>> AddAsync(Friends data)
        {
            var request = new Friends
            {
                FriendIds = data.FriendIds
            };

            await _friendsRepository.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RemoveFriendAsync(string id)
        {
            var existingItem = await _friendsRepository.GetAsync(id);
            await _friendsRepository.UpdateAsync(existingItem);
            return NoContent();
        }
    }
}
