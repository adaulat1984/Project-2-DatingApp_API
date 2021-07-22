using DatingApp_API.Data;
using DatingApp_API.DTO;
using DatingApp_API.Entities;
using DatingApp_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public ITokenService _tokenService { get; }

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            if (await UserExists(register.UserName)) { return BadRequest("UserName already exist"); } 
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                Username = register.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes( register.Password)),
                PasswordSalt = hmac.Key

            };
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return new UserDTO
            {
                UserName = user.Username,
                Token = _tokenService.CreateTokenSerive(user)
            };
        
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login) {
            var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.Username == login.UserName.ToLower());
            if(user == null)
            { return Unauthorized("userame is incorrect"); }
             using var hmac = new HMACSHA512(user.PasswordSalt);
            var PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            for (int i = 0; i < PasswordHash.Length; i++)
            {
                if (PasswordHash[i] == user.PasswordHash[i])
                    return Unauthorized("Password is incorrect");
            }

            return new UserDTO
            {
                UserName = user.Username,
                Token = _tokenService.CreateTokenSerive(user)
            };

        }
        private async Task<bool> UserExists(string username) {
            return await _dataContext.Users.AnyAsync(x => x.Username == username.ToLower());
        }
    }
}
