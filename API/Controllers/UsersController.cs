using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;


namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        private readonly IDistributedCache _redis;

        public UsersController(IUserRepository userRepository, IMapper mapper, IDistributedCache redis)
        {
            _mapper = mapper; 
            _userRepository = userRepository;
            _redis = redis;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var cachedUsers = await _redis.GetStringAsync("users");

            IEnumerable<MemberDto>? users;

            if (!string.IsNullOrWhiteSpace(cachedUsers))
            {
                users = JsonConvert.DeserializeObject<IEnumerable<MemberDto>>(cachedUsers);
                return Ok(users);
            }
            users = await _userRepository.GetMembersAsync();

            await _redis.SetStringAsync("users", JsonConvert.SerializeObject(users));

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

            await _redis.SetStringAsync(username, JsonConvert.SerializeObject(user));

            return user;

        }

    }
}
