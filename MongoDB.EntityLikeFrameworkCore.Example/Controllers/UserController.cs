using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.EntityLikeFrameworkCore.Example.Core;
using MongoDB.EntityLikeFrameworkCore.Example.Models;
using System.Threading.Tasks;

namespace MongoDB.EntityLikeFrameworkCore.Example.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository repository;

        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}", Name ="GetUserById")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await repository.FindOne(id);

            if (user == null)
                return NotFound();
            else
                return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User value)
        {
            if (value == null)
                return BadRequest();

            await repository.InertUser(value);
            return CreatedAtRoute("GetUserById", new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]User value)
        {
            if (value == null)
                return BadRequest();

            var result = await repository.UpdateUser(id, value);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]JsonPatchDocument<User> patcher)
        {
            if (patcher == null)
                return BadRequest();

            var target = await repository.FindOne(id);
            if (target == null)
                return NotFound();

            patcher.ApplyTo(target);

            await repository.UpdateUser(id, target);

            return Ok(target);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await repository.DeleteUser(id);
            return NoContent();
        }
    }
}
