using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
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
        private readonly IPhotoService photoService;

        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {           
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.photoService = photoService;
        }

        //[AllowAnonymous]
        [HttpGet] 
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //  var users = await userRepository.GetUsersAsync();
            //  var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users); // Map from AppUser to MemberDto
            var usersToReturn = await userRepository.GetMembersAsync();
            return  Ok(usersToReturn); // for get we return ok 200 response
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
            var username = User.GetUsername(); // Get current user by getting  username from token , we have used username as JwtRegisteredClaimNames.NameId in TokenService 
            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound();
            
            mapper.Map(memberUpdateDto, user);
            userRepository.Update(user);
            if (await userRepository.SaveAllAsync()) return NoContent(); // for put (update) we return NoContent 204 response
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();
            
            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri, // AbsoluteUri is used to get full url of image (path)
                PublicId = result.PublicId // PublicId is used to delete image from cloudinary
            };

            if (user.Photos.Count == 0) // if the user don't have photos set this as main 
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await userRepository.SaveAllAsync())
            {
                // for post CreatedAtAction is used to return 201 response and in header the location url of new result and the third parameter is the result of the action (photo)
                return CreatedAtAction(nameof(GetUsers),
                   new { username = user.UserName }, mapper.Map<PhotoDto>(photo));  
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }



    }
}
