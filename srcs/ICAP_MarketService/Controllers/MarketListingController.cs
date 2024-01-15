using ICAP_Infrastructure.Repositories;
using ICAP_MarketService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ICAP_MarketService.Controllers
{
    
    [ApiController]
    [Route("listings")]
    public class MarketListingController(IRepository<MarketListing> listingRepository) : ControllerBase
    {
        public record MarketListingDto(string Title, string Description);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarketListing>>> GetAsync()
        {
            var items = await listingRepository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MarketListing>> GetByIdAsync(string id)
        {
            var item = await listingRepository.GetAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpPost]
        public async Task<ActionResult<MarketListing>> AddAsync(MarketListingDto reqData)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;

            var data = new MarketListing
            {
                Id = Guid.NewGuid().ToString(),
                UserId = oid,
                Title = reqData.Title,
                Description = reqData.Description,
                CreatedDateTime = DateTimeOffset.Now
            };
            var userResult = await listingRepository.GetAllAsync(listing => listing.Title == reqData.Title && listing.UserId == oid);
            if (userResult.Count > 0) return Conflict("A listing for your user with the same title already exists.");
            await listingRepository.CreateAsync(data);
            return Created(Request.Path, data);
        }

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditListingAsync(MarketListingDto reqData, string id)
        {
            var existingItem = await listingRepository.GetAsync(id);
            if (existingItem is null) return NotFound();
            existingItem.Description = reqData.Description;
            existingItem.Title = reqData.Title;
            await listingRepository.UpdateAsync(existingItem);
            return Ok();
        }

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveListingAsync(string id)
        {
            var existingItem = await listingRepository.GetAsync(id);
            if (existingItem is null) return NotFound();
            await listingRepository.RemoveAsync(id);
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
