using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ICAP_AccountService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController(IRepository<User> usersRepository, IBus bus) : ControllerBase
    {
        public record UserDto(string Name, string Email);

        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            var items = await usersRepository.GetAllAsync();
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetByIdAsync(string id)
        {
            var item = await usersRepository.GetAsync(id);
            return item;
        }

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpPost]
        public async Task<ActionResult<User>> PostAsync()
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);

            var id = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var name = decodedToken.Claims.First(claim => claim.Type == "name").Value;
            var email = decodedToken.Claims.First(claim => claim.Type == "email").Value;

            var request = new User
            {
                Id = id,
                Name = name,
                Email = email,
                CreatedDateTime = DateTimeOffset.Now
            };
            var userResult = await usersRepository.GetAllAsync(user => user.Email == request.Email);
            if (userResult.Count > 0) return Conflict("User with this email already exists.");

            await usersRepository.CreateAsync(request);
            return Created(Request.Path, request);
        }

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpPut]
        public async Task<IActionResult> PutAsync(UserDto data)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;

            var existingItem = await usersRepository.GetAsync(oid);
            if (existingItem is null) return NotFound();
            existingItem.Name = data.Name;
            existingItem.Email = data.Email;
            
            await usersRepository.UpdateAsync(existingItem);

            return Ok();
        }

        public record DeleteUserData(string UserId);

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {

            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            if (oid.IsNullOrEmpty()) return BadRequest("Unable to get OID from token");

            await bus.Publish(new DeleteUserData(oid));
            var existingItem = await usersRepository.GetAsync(oid);
            if (existingItem == null) return NotFound("User was not found");
            await usersRepository.RemoveAsync(existingItem.Id);
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
