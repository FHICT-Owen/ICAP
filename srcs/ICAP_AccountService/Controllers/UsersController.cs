using ICAP_AccountService.Entities;
using ICAP_Infrastructure.MTDtos;
using ICAP_Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ICAP_AccountService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController(IRepository<User> usersRepository, IBus bus) : ControllerBase
    {
        public record UserDto(string UserId, string GivenName, string Email);

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            var items = await usersRepository.GetAllAsync();
            return items.Select(users => new UserDto(users.Id, users.Name, ""));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetByIdAsync(string id)
        {
            var item = await usersRepository.GetAsync(id);
            if (item == null) return NotFound();
            return new UserDto(item.Id, item.Name, "");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostAsync()
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);

            var id = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            var name = decodedToken.Claims.First(claim => claim.Type == "name").Value;
            var email = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

            var request = new User
            {
                Id = id,
                Name = name,
                Email = email ?? "No Email Found",
                CreatedDateTime = DateTimeOffset.Now
            };
            var userResult = await usersRepository.GetAllAsync(user => user.Email == request.Email);
            if (userResult.Count > 0) return Conflict("User with this email already exists.");

            await usersRepository.CreateAsync(request);
            return Created(Request.Path, request);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutAsync(UserDto data)
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;

            var existingItem = await usersRepository.GetAsync(oid);
            if (existingItem is null) return NotFound();
            existingItem.Name = data.GivenName;
            existingItem.Email = data.Email;
            
            await usersRepository.UpdateAsync(existingItem);

            return Ok();
        }

        [Authorize]
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
