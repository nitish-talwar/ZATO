using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZATO.Contracts;
using ZATO.DTOs;
using ZATO.Models;

namespace ZATO.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    
    public UserController(IMapper mapper,
        IUserRepository userRepository)
    {
        this._mapper = mapper;
        this._userRepository = userRepository;
    }
    // GET
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDetailsDTO>> Get(Guid userId)
    {
        var user = await this._userRepository.GetAsync(userId);

        if (user == null)
        {
            throw new Exception($"User {userId} is not found.");
        }

        var userDetailsDto = _mapper.Map<UserDetailsDTO>(user);

        return Ok(userDetailsDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDetailsDTO>>> GetAllUsers()
    {
        var userList = await this._userRepository.GetAllAsync();
        if (userList is null)
        {
            userList = new List<User>();
        }

        var userListDto = _mapper.Map<List<UserDetailsDTO>>(userList);
        return Ok(userListDto);
    }
    [HttpPost]
    public async Task<ActionResult<User>> Register(UserRegistrationDTO urd)
    {
        if (urd is not null)
        {
            var user = new User();
            user.UserName = urd.FirstName + urd.LastName;
            user.Email = urd.Email;
            user.PasswordHash = urd.Password;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            await this._userRepository.CreateAsync(user);
        }
        return Ok(urd);
    }

    [HttpDelete]
    public async Task<int> Delete(Guid id)
    {
       return await this._userRepository.DeleteAsync(id);
    }
}