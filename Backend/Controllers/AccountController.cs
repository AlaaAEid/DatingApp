using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ILogger<BaseController> logger, ITokenService tokenService) : base(context, logger)
        {
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if ( await userExists(registerDto.username)) 
            {
                return BadRequest("Username is Taken");
            }

            using var hmac = new HMACSHA512();

            var user = new User{
                UserName = registerDto.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key
            };

            

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return new UserDto{
                username = registerDto.username.ToLower(),
                token = _tokenService.createToken(user),
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) 
        {
            var user = await _context.Users.SingleOrDefaultAsync( user => user.UserName == loginDto.username.ToLower());
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            for (int i = 0 ; i < hash.Length ; i ++) {
                if ( hash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto{
                username = loginDto.username.ToLower(),
                token = _tokenService.createToken(user),
            };
        }

        private async Task<bool> userExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}