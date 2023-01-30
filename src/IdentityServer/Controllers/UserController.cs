using IdentityInfrastructure.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

public class UserController : ControllerBase
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<UserEntity> _userManager;

    public UserController(UserManager<UserEntity> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("api/user")]
    public async Task<IActionResult> Get()
    {
        var us = await _userManager.AddToRoleAsync(_userManager.Users.First(), "User");
        return Ok(us);
    }

    [HttpPost]
    [Route("api1/user")]
    public async Task<IActionResult> Post(UserRegistrationRequest request)
    {
        var user = new UserEntity
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            SecondName = request.SecondName
        };
        var result = await _userManager.CreateAsync(user, request.Password!);
        if (result.Succeeded) return Ok();
        return BadRequest(result.Errors);
    }

    [HttpPut]
    [Route("api1/user")]
    public async Task<IActionResult> Put(UserUpdateRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id!);
        if (user == null) return BadRequest("User not found");
        user.FirstName = request.FirstName;
        user.SecondName = request.SecondName;
        user.PhoneNumber = request.PhoneNumber;
        user.Email = request.Email;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded) return Ok();
        return BadRequest(result.Errors);
    }

    public class UserUpdateRequest
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }


    public class UserRegistrationRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}