using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Security.Claims;
using API.Extensions;
using API.Entities;
using API.Helpers;


namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        private readonly IDistributedCache _redis;

        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IDistributedCache redis, IPhotoService photoService)
        {
            _mapper = mapper; 
            _userRepository = userRepository;
            _redis = redis;
            _photoService = photoService;
        }

        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUser = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            userParams.CurrentUsername = currentUser.UserName;

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{username}")] // /api/users/username
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {

            var cachedUser = await _redis.GetStringAsync(username);

            MemberDto? user;

            if (!string.IsNullOrWhiteSpace(cachedUser))
            {
                user = JsonConvert.DeserializeObject<MemberDto>(cachedUser);
                return user;
            }

            user = await _userRepository.GetMemberAsync(username);

            if (user == null) return NotFound();

            await _redis.SetStringAsync(username, JsonConvert.SerializeObject(user));

            return user;

        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // "User" currently authenticated user
            var username = User.GetUsername();

            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            _redis.Remove(username);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user.");
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto (IFormFile file)
        {
            var username = User.GetUsername();
            
            var user = await _userRepository.GetUserByUsernameAsync(username);
            
            if(user==null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null ) return BadRequest(result.Error);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user?.Photos?.Count == 0) photo.IsMain = true;

            user?.Photos?.Add(photo);

            _redis.Remove(username);

            if (await _userRepository.SaveAllAsync()) 
            {
                return CreatedAtAction(nameof(GetUser),
                    new { username = user?.UserName }, _mapper.Map<PhotoDto>(photo));
                    // returns the location with the route parameter username in the headers and the new photo in the body
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUsername();

            var user = await _userRepository.GetUserByUsernameAsync(username);

            if(user==null) return NotFound();

            var photo = user?.Photos?.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user?.Photos?.FirstOrDefault(x => x.IsMain);

            if(currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            _redis.Remove(username);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo.");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();

            var user = await _userRepository.GetUserByUsernameAsync(username);

            if(user==null) return NotFound();

            var photo = user?.Photos?.FirstOrDefault(x => x.Id == photoId);

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsyc(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user?.Photos?.Remove(photo); // Just remove from the list of photos, EF will take care of the rest

            if (await _userRepository.SaveAllAsync()) return Ok();

            _redis.Remove(username);

            return BadRequest("Failed to delete the photo");
        }
    }
}
