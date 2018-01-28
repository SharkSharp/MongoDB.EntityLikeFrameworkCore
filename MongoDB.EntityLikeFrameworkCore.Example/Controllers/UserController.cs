using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Example.Core;
using MongoDB.EntityLikeFrameworkCore.Example.Models;
using System.Linq;

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
        public IActionResult Get()
        {
            return Ok(repository.GetAll());
        }

        [HttpGet("{id}", Name ="GetUserById")]
        public IActionResult Get(string id)
        {
            var user = repository.FindOne(id);

            if (user == null)
                return NotFound();
            else
                return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody]User value)
        {
            if (value == null)
                return BadRequest();

            repository.InertUser(value);
            return CreatedAtRoute("GetUserById", new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]User value)
        {
            if (value == null)
                return BadRequest();

            var result = repository.UpdateUser(id, value);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public IActionResult Put(string id, [FromBody]JsonPatchDocument<User> patcher)
        {
            if (patcher == null)
                return BadRequest();

            var target = repository.FindOne(id);
            if (target == null)
                return NotFound();

            patcher.ApplyTo(target);

            repository.UpdateUser(id, target);

            return Ok(target);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            repository.DeleteUser(id);
            return NoContent();
        }
    }
}
