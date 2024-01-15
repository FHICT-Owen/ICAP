using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ICAP_AccountService.Controllers
{
    [Authorize]
    [RequiredScope("access_as_user")]
    [ApiController]
    [Route("users")]
    public class UsersController(IRepository<User> usersRepository, IBus bus) : ControllerBase
    {
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

        [HttpPut]
        public async Task<IActionResult> PutAsync(User data)
        {
            var existingItem = await usersRepository.GetAsync(data.Id);
            existingItem.Name = data.Name;
            existingItem.Email = data.Email;
            
            await usersRepository.UpdateAsync(existingItem);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.IsNullOrEmpty()) return BadRequest();
            var decodedToken = GetTokenFromAuthHeader(authorizationHeader);
            var oid = decodedToken.Claims.First(claim => claim.Type == "oid").Value;
            if (oid == null) return BadRequest("Unable to get OID from token");

            var endpoint = await bus.GetSendEndpoint(new Uri("topic:deleteuserdata"));
            await endpoint.Send(oid);

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
