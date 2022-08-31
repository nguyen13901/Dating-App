using System.Text;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Database;
using DatingApp.API.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.DTOs;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        private readonly DataContext _context;

        public AccountsController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("register")]
        public ActionResult<String> Register(RegisterDto registerDto)
        {
            registerDto.Username.ToLower();
            if (_context.Users.Any(u => u.Username == registerDto.Username)) {
                return BadRequest("Username already is register");
            }

            using var hmac = new HMACSHA512();

            var user = new User {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<String> Login(LoginDto loginDto)
        {
            loginDto.Username.ToLower();
            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (var i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.PasswordHash[i]) {
                    return Unauthorized("Invalid password");
                }
            }
            return Ok("Login successfully");
        }
    }
}