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
    [Route("friends")]
    public class FriendsController(IRepository<Friends> friendsRepository) : ControllerBase
    {
        public record FriendDto(List<string> FriendIds);
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
        public async Task<ActionResult<Friends>> AddAsync(FriendDto request)
        {
            var data = new Friends
            {
                FriendIds = request.FriendIds
            };

            await friendsRepository.CreateAsync(data);
            return Created(Request.Path, data);
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
