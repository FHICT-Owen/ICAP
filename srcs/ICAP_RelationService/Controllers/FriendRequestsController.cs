using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var id = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var items = await friendRequestsRepository.GetAllAsync(request => request.UserFrom == id);
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
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var id = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var request = new FriendRequest
            {
                Id = Guid.NewGuid().ToString(),
                UserFrom = id,
                UserTo = data.UserTo,
                RequestStatus = FriendRequestStatus.Pending,
                CreatedDateTime = DateTimeOffset.Now
            };

            await friendRequestsRepository.CreateAsync(request);
            return Created(Request.Path, request);
        }

        [HttpPut("Accept/{id}")]
        public async Task<IActionResult> AcceptAsync(string userFromId)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var id = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var existingItem = await friendRequestsRepository.GetAsync(request => request.UserTo == id
                && request.UserFrom == userFromId);
            if (existingItem is null) return NotFound();
            existingItem.RequestStatus = FriendRequestStatus.Accepted;
            await friendRequestsRepository.UpdateAsync(existingItem);
            return Ok();
        }

        [HttpPut("Decline/{id}")]
        public async Task<IActionResult> DeclineAsync(string userFromId)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var id = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var existingItem = await friendRequestsRepository.GetAsync(request => request.UserTo == id
                && request.UserFrom == userFromId);
            if (existingItem is null) return NotFound();
            existingItem.RequestStatus = FriendRequestStatus.Declined;
            await friendRequestsRepository.UpdateAsync(existingItem);
            return Ok();
        }

        private JwtSecurityToken GetTokenFromAuthHeader(string authorizationHeader)
        {
            var token = authorizationHeader["Bearer ".Length..].Trim();
            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.ReadJwtToken(token);
        }
    }
}
