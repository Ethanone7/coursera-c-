using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // 导入数据验证命名空间
using System.Linq;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new();

        public UsersController()
        {
            if (users.Count == 0)
            {
                users.Add(new User { Id = 1, Name = "Test User 1", Email = "test1@email.com" });
                users.Add(new User { Id = 2, Name = "Test User 2", Email = "test2@email.com" });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return users;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                var user = users.Find(u => u.Id == id);
                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." }); // 提供更详细的错误信息
                }
                return user;
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the user.", error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<User> PostUser([FromBody] User user) // [FromBody] 注解很重要
        {
            if (!ModelState.IsValid) // 检查模型验证是否通过
            {
                return BadRequest(ModelState); // 返回验证错误信息
            }
            try{
                user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
                users.Add(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            } catch (System.Exception ex){
                return StatusCode(500, new { message = "An error occurred while creating the user.", error = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public IActionResult PutUser(int id, [FromBody] User user) // [FromBody] 注解很重要
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            try
            {
                var existingUser = users.Find(u => u.Id == id);
                if (existingUser == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = users.Find(u => u.Id == id);
                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                users.Remove(user);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the user.", error = ex.Message });
            }
        }
    }
}