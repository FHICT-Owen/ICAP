using ICAP_MarketService.Collections;
using ICAP_MarketService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ICAP_MarketService.Controllers
{
    public record MarketListingDto(string Title, string Description, double Price, ServiceCategory Category, string ImageLink);

    [ApiController]
    [RequiredScope("access_as_user")]
    [Route("listings")]
    public class MarketListingController(MarketListingCollection listings) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarketListing>>> GetAsync()
        {
            var items = await listings.GetAllListingsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MarketListing>> GetByIdAsync(string id)
        {
            var item = await listings.GetListingAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MarketListing>> AddAsync(MarketListingDto reqData)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;

            var result = await listings.AddListingAsync(reqData, oid);
            return result.Match<ActionResult>(
                successResult => Created(Request.Path, successResult.Entity), 
                failureResult => BadRequest(failureResult.HelperText));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(MarketListingDto reqData, string id)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var isAdmin = decodedToken.Claims.First(claim => claim.Type == "roles").Value.Contains("access_as_admin");
            var listing = await listings.GetListingAsync(id);
            if (listing?.UserId != oid || !isAdmin)
                return Unauthorized("You can not edit a listing belonging to another user.");

            var result = await listings.EditListingAsync(reqData, id);

            return result.Match<IActionResult>(
                successResult => Ok(successResult.Entity),
                failureResult => BadRequest(failureResult.HelperText));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(string id)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var isAdmin = decodedToken.Claims.First(claim => claim.Type == "roles").Value.Contains("access_as_admin");
            var listing = await listings.GetListingAsync(id);
            if (listing?.UserId != oid || !isAdmin)
                return Unauthorized("You can not remove a listing belonging to another user.");

            var result = await listings.RemoveListingAsync(id);
            return result ? Ok() : NotFound("Could not find listing with corresponding ID");
        }

        private JwtSecurityToken GetTokenFromAuthHeader(string authorizationHeader)
        {
            var token = authorizationHeader["Bearer ".Length..].Trim();
            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.ReadJwtToken(token);
        }
    }
}
