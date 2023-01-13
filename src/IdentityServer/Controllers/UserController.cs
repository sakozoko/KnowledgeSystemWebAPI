using IdentityServer.Models;
using IdentityServer.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Controllers;

public class UserController : ControllerBase
{
    private readonly SignInManager<UserEntity> _signInManager;

    public UserController(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }
    [HttpGet]
    [Route("api1/user")]
    public async Task<IActionResult> Get()
    {
        User.Claims.ToList();
        return Ok(await _signInManager.UserManager.CreateAsync(new UserEntity()
        {
            UserName = "testUserName",
            Email = "testemail@gmail.com",
        }, "testPassword1+"));
    }
}