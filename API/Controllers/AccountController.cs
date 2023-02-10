using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository
            ,IMapper mapper)
        {
            this.context = context;
            this.tokenService = tokenService;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPost("register")] // api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            

            if (await UserExist(registerDto.Username)) return BadRequest("username is taken");

            var user = mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();


            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
           

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token=tokenService.CreateToken(user),
                KnownAs= user.KnownAs
            };
        }

        [HttpPost("login")] // api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            await context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };

        }

        private async Task<bool> UserExist(string username)
        {
            return await context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
