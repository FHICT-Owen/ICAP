﻿using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICAP_AccountService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _usersRepository;

        public UsersController(IRepository<User> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            var items = await _usersRepository.GetAllAsync();
            return items;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<User>> GetByIdAsync(string id)
        {
            var item = await _usersRepository.GetAsync(id);
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostAsync(User data)
        {
            var request = new User
            {
                Name = data.Name,
                Email = data.Email,
                CreatedDateTime = DateTimeOffset.Now
            };

            await _usersRepository.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = request.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, User data)
        {
            var existingItem = await _usersRepository.GetAsync(id);
            if (existingItem == null) return NotFound();
            existingItem.Name = data.Name;
            existingItem.Email = data.Email;
            
            await _usersRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var existingItem = await _usersRepository.GetAsync(id);
            await _usersRepository.RemoveAsync(existingItem.Id);
            return NoContent();
        }
    }
}
