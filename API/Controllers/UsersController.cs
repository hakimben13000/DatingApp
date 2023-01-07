using API.Data;
using API.DTOs;
using API.Entities;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    }
}
