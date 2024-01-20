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
    [Route("friendrequests")]
    public class FriendRequestsController(IRepository<FriendRequest> friendRequestsRepository) : ControllerBase
    {
        public record FriendRequestDto(string UserFrom, string UserTo);
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendRequest>>> GetAsync()
        {
            var items = await friendRequestsRepository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FriendRequest>> GetByIdAsync(string id)
        {
            var item = await friendRequestsRepository.GetAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> SendAsync(FriendRequestDto data)
        {
            var request = new FriendRequest
            {
                Id = Guid.NewGuid().ToString(),
                UserFrom = data.UserFrom,
                UserTo = data.UserTo,
                RequestStatus = FriendRequestStatus.Pending,
                CreatedDateTime = DateTimeOffset.Now
            };

            await friendRequestsRepository.CreateAsync(request);
            return Created(Request.Path, request);
        }

        [HttpPut("Accept/{id}")]
        public async Task<IActionResult> AcceptAsync(string id)
        {
            var existingItem = await friendRequestsRepository.GetAsync(id);
            if (existingItem is null) return NotFound();
            existingItem.RequestStatus = FriendRequestStatus.Accepted;
            await friendRequestsRepository.UpdateAsync(existingItem);
            return Ok();
        }

        [HttpPut("Decline/{id}")]
        public async Task<IActionResult> DeclineAsync(string id)
        {
            var existingItem = await friendRequestsRepository.GetAsync(id);
            if (existingItem is null) return NotFound();
            existingItem.RequestStatus = FriendRequestStatus.Declined;
            await friendRequestsRepository.UpdateAsync(existingItem);
            return Ok();
        }
    }
}
