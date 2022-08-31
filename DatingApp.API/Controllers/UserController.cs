using DatingApp.API.Database;
using DatingApp.API.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class UserController: BaseApiController
    {
        private readonly DataContext _context;

        public UserController(DataContext context) {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get(){
            return Ok(_context.Users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id) {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) {
                return NotFound();
            }
            return Ok(user);
        }
    }
}