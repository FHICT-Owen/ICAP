using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace ICAP_RelationService.Controllers
{
    [Authorize]
    [RequiredScope("access_as_user")]
    [ApiController]
    [Route("[controller]")]
    public class FriendsController(IRepository<Friends> friendsRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friends>>> GetAsync()
        {
            var items = await friendsRepository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Friends>> GetByIdAsync(string id)
        {
            var item = await friendsRepository.GetAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Friends>> AddAsync(Friends data)
        {
            var request = new Friends
            {
                FriendIds = data.FriendIds
            };

            await friendsRepository.CreateAsync(request);
            return CreatedAtRoute(nameof(GetByIdAsync), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RemoveFriendAsync(string id)
        {
            var existingItem = await friendsRepository.GetAsync(id);
            if (existingItem is null) return NotFound();
            await friendsRepository.UpdateAsync(existingItem);
            return Ok();
        }
    }
}
