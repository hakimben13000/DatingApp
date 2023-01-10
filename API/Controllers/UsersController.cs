using API.Data;
using API.DTOs;
using API.Entities;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository,IMapper mapper)
        {           
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //  var users = await userRepository.GetUsersAsync();
            //  var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users); // Map from AppUser to MemberDto
            var usersToReturn = await userRepository.GetMembersAsync();
            return  Ok(usersToReturn);
        }

  
      /*  [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await userRepository.GetUserByIdAsync(id);
        }
      */
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUserName(string username)
        {
            // var user = await userRepository.GetUserByUsernameAsync(username); // Get user from database but if we don't use mapper we got circular reference of AppUser and Photo
            //var userToReturn = mapper.Map<MemberDto>(user); // Map from AppUser to MemberDto
            return await userRepository.GetMemberAsync(username);
            
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser( MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get current user by getting  username from token , we have used username as JwtRegisteredClaimNames.NameId in TokenService 
            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound();
            
            mapper.Map(memberUpdateDto, user);
            userRepository.Update(user);
            if (await userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }


    }
}
